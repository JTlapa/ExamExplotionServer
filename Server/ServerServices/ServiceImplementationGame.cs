﻿using DataAccess;
using DataAccess.EntitiesManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerService
{
    public partial class ServiceImplementation : IGameManager
    {
        private Dictionary<string, string> currentTurnByGame = new Dictionary<string, string>();
        private Dictionary<string, List<string>> playersInGame = new Dictionary<string, List<string>>();
        private static Dictionary<string, Dictionary<string, IGameConnectionCallback>> gameConnections = new Dictionary<string, Dictionary<string, IGameConnectionCallback>>();
        private static Dictionary<string, bool> turnTransitionState = new Dictionary<string, bool>();
        private static Dictionary<string, List<string>> doubleTurns = new Dictionary<string, List<string>>();
        private static Dictionary<string, bool> deckInitialized = new Dictionary<string, bool>();
        public void InitializeDeck(string gameCode, int playerCount, string gamertag)
        {
            List<CardManagement> cards = new List<CardManagement>();
            AddCardToList(cards, "Ver el futuro", "viewTheFuture", 6);
            AddCardToList(cards, "Dejo el equipo", "leftTeam", 6);
            AddCardToList(cards, "Exentar", "exempt", 5);
            AddCardToList(cards, "Paro", "please", 6);
            AddCardToList(cards, "Revolver", "shuffle", 6);
            AddCardToList(cards, "Agarrar de abajo", "takeFromBelow", 7);
            AddCardToList(cards, "El profe R", "profeR", 3);
            AddCardToList(cards, "El profe O", "profeO", 3);
            AddCardToList(cards, "El profe S", "profeS", 3);
            AddCardToList(cards, "El profe A", "profeA", 3);
            AddCardToList(cards, "El profe M", "profeM", 3);

            List<CardManagement> shuffledDeck = cards.OrderBy(cardDeck => Guid.NewGuid()).ToList();
            Stack<CardManagement> gameDeck = new Stack<CardManagement>(shuffledDeck);

            Console.WriteLine($"Se inicio el stack del juego {gameCode}");
            SendPlayerAndGameDeck(gameDeck, gameCode, playerCount);
        }
        private void SendPlayerAndGameDeck(Stack<CardManagement> gameDeck, string gameCode, int playerCount)
        {
            List<CardManagement>[] playerDecks = {null, null, null, null};
            for(int i = 0; i < playerCount; i++)
            {
                List<CardManagement> playerDeck = new List<CardManagement>();
                CardManagement card = new CardManagement();
                card.CardName = "Re Registration";
                card.CardPath = "reRegistration";
                playerDeck.Add(card);
                for (int j = 0; j < 7; j++)
                {
                    card = gameDeck.Pop();
                    playerDeck.Add(card);
                }
                playerDecks[i] = playerDeck;
            }
            Stack<CardManagement> finalGameDeck = FinalizeGameDeck(gameDeck, playerCount);
            if (gameConnections.ContainsKey(gameCode))
            {
                int index = 0;
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    player.Value.RecivePlayerAndGameDeck(finalGameDeck, playerDecks[index]);
                    index++;
                }
                if (!deckInitialized.ContainsKey(gameCode))
                {
                    deckInitialized[gameCode] = true;
                }
            }
        }

        public void SendShuffleDeck(string gameCode, Stack<CardManagement> gameDeck)
        {
            if (gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    player.Value.ReceiveGameDeck(gameDeck);
                }
            }
        }

        private Stack<CardManagement> FinalizeGameDeck(Stack<CardManagement> gameDeck, int playerCount)
        {
            int repiteCount = Math.Max(0, playerCount - 1);
            int reinscripcionCount = Math.Max(0, 6 - playerCount);
            List<CardManagement> cards = gameDeck.ToList();
            AddCardToList(cards, "Repite", "examBomb", repiteCount);
            AddCardToList(cards, "Reinscripcion", "reRegistration", reinscripcionCount);
            var shuffledDeck = cards.OrderBy(cardDeck => Guid.NewGuid()).ToList();
            var newGameDeck = new Stack<CardManagement>(shuffledDeck);
            return newGameDeck;
        }
        private void AddCardToList(List<CardManagement> cardList, string cardName, string cardPath, int count)
        {
            for (int i = 0; i < count; i++)
            {
                cardList.Add(new CardManagement { CardName = cardName, CardPath = cardPath});
            }
        }
        public string GetCurrentTurn(string gameCode)
        {
            return currentTurnByGame.ContainsKey(gameCode) ? currentTurnByGame[gameCode] : null;
        }

        public GameManagement GetGame(string gameCode)
        {
            GameManagement gameModel = new GameManagement();
            Game gameObtained = GameManagerDB.getGameByCode(gameCode);
            if (gameObtained != null)
            {
                gameModel.GameId = gameObtained.gameId;
                gameModel.HostPlayerId = gameObtained.hostPlayerId;
                gameModel.InvitationCode = gameObtained.invitationCode;
                gameModel.TimePerTurn = (int)gameObtained.timePerTurn;
                gameModel.Lives = (int)gameObtained.lives;
                gameModel.NumberPlayers = (int)gameObtained.numberPlayers;
                return gameModel;
            }
            else
            {
                return null;
            }
        }
        public void InitializeGameTurns(string gameCode, List<string> gamertags)
        {
            this.playersInGame[gameCode] = gamertags.OrderBy(_ => Guid.NewGuid()).ToList();
            var firstPlayer = playersInGame[gameCode].First();
            currentTurnByGame[gameCode] = firstPlayer;
            turnTransitionState[gameCode] = false;
            NotifyClientOfTurn(gameCode, firstPlayer);
        }

        public void NotifyClientOfTurn(string gameCode, string nextGametag)
        {
                if (gameConnections.ContainsKey(gameCode))
                {
                    var playesInTheGame = gameConnections[gameCode];
                    foreach (var player in playesInTheGame)
                    {
                        player.Value.UpdateCurrentTurn(nextGametag);
                        player.Value.SyncTimer();
                    }
                    ResetTurnTransitionState(gameCode);
                }
        }

        public void NotifyEndTurn(string gameCode, string currentGamertag)
        {
            if (turnTransitionState.ContainsKey(gameCode) && !turnTransitionState[gameCode])
            {
                if (playersInGame.ContainsKey(gameCode))
                {
                    var players = playersInGame[gameCode];
                    int currentIndex = players.IndexOf(currentGamertag);

                    foreach(var p in players)
                    {
                        Console.WriteLine(p);
                    }

                    int nextIndex = (currentIndex + 1) % players.Count;
                    var nextGametag = players[nextIndex];
                    if (doubleTurns.ContainsKey(gameCode))
                    {
                        var playersWithDoubleTurns = doubleTurns[gameCode];
                        if (playersWithDoubleTurns.Contains(currentGamertag))
                        {
                            nextGametag = currentGamertag;
                            nextIndex = currentIndex;
                            doubleTurns[gameCode].Remove(nextGametag);
                        }
                        else if (playersWithDoubleTurns.Contains("next"))
                        {
                            doubleTurns[gameCode].Remove("next");
                            doubleTurns[gameCode].Add(nextGametag);
                        }
                    }

                    currentTurnByGame[gameCode] = nextGametag;
                    turnTransitionState[gameCode] = true;
                    NotifyClientOfTurn(gameCode, nextGametag);
                }
            }
        }

        public void ResetTurnTransitionState(string gameCode)
        {
            if (turnTransitionState.ContainsKey(gameCode))
            {
                turnTransitionState[gameCode] = false;
            }
        }
        public bool ConnectGame(string gameCode, string gamertag)
        {
            
            var callback = OperationContext.Current.GetCallbackChannel<IGameConnectionCallback>();
            bool connected = false;
            if (!gameConnections.ContainsKey(gameCode))
            {
                gameConnections[gameCode] = new Dictionary<string, IGameConnectionCallback>();
            } 
            gameConnections[gameCode][gamertag] = callback;
            connected = true;
            if (deckInitialized.ContainsKey(gameCode) && deckInitialized[gameCode] == true)
            {
                connected = false;
            }
            return connected;
        }

        public void NotifyDrawCard(string gameCode, string gamertag, bool isTopCard)
        {
            if (gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    if(player.Key != gamertag)
                    {
                        player.Value.RemoveCardFromStack(isTopCard);
                    }
                }
                ResetTurnTransitionState(gameCode);
            }
        }
        public void NotifyCardOnBoard(string gameCode, string path)
        {
            if(gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    player.Value.PrintCardOnBoard(path);
                }
            }
        }

        public void RequestCard(string gameCode, string playerRequested, string playerRequesting)
        {
            if (gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    if (player.Key.Equals(playerRequested))
                    {
                        player.Value.NotifyCardRequested(gameCode, playerRequesting);
                    }
                }
            }
        }

        public void SendCardToPlayer(string gameCode, string playerRequesting, CardManagement cardToSend)
        {
            if (gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    if (player.Key.Equals(playerRequesting))
                    {
                        player.Value.NotifyCardReceived(cardToSend);
                    }
                }
            }
        }
        public void NotifyMessage(string gameCode, string gamertagOrigin, string gamertagDestination, string message)
        {
            if (gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                if(gamertagDestination == "all")
                {
                    foreach (var player in playesInTheGame)
                    {
                        if (player.Key != gamertagOrigin)
                        {
                            player.Value.ReciveNotification(message);
                        }
                    }
                }
                else
                {
                    if (playersInGame.ContainsKey(gamertagDestination))
                    {
                        playesInTheGame[gamertagDestination].ReciveNotification(message);
                    }
                }
            }
        }
        public void RemovePlayerByGame(string gameCode, string gamertag)
        {
            if (playersInGame.ContainsKey(gameCode))
            {
                var players = playersInGame[gameCode];
                if (players.Contains(gamertag))
                {
                    players.Remove(gamertag);
                    playersInGame[gameCode] = players;
                    if(players.Count <= 1)
                    {
                        NotifyThereIsAWinner(gameCode, gamertag);
                    }
                }
            }
        }
        private void NotifyThereIsAWinner(string gameCode, string gamertag)
        {
            string winnerGamertag = gamertag;
            if (playersInGame[gameCode].Count > 0)
            {
                winnerGamertag = playersInGame[gameCode][0];
            }
            if (gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    player.Value.EndTheGame(gameCode, winnerGamertag);
                }
            }
            SaveWinner(gameCode, winnerGamertag);
        }

        private void SaveWinner(string gameCode, string winnerGamertag)
        {
            int userId = -1;
            if (!winnerGamertag.StartsWith("GUEST"))
            {
                var playerObtained = PlayerManagerDB.GetPlayerByGamertag(winnerGamertag);
                if(playerObtained != null)
                {
                    userId = playerObtained.userId;
                }
            }
            else
            {
                int guestId = int.Parse(winnerGamertag.Substring(6));
                userId = GuestManagerDB.GetUserIdByGuestId(guestId);
            }
            if (userId > 0)
            {
                GameManagerDB.UpdateWinner(gameCode, userId);
                if (!winnerGamertag.StartsWith("GUEST"))
                {
                    PlayerManagerDB.UpdateScore(userId, 100);
                    PlayerManagerDB.AddWin(userId);
                }
            }
        }

        public void SendExamBomb(string gameCode, int gameDeckCount)
        {
            Random random = new Random();
            int index = random.Next(0, gameDeckCount);
            if (gameConnections.ContainsKey(gameCode))
            {
                var playesInTheGame = gameConnections[gameCode];
                foreach (var player in playesInTheGame)
                {
                    player.Value.ReciveAndAddExamBomb(index);
                }
            }
        }
        public void SendDoubleTurn(string gameCode, string gamertag)
        {
            if (!doubleTurns.ContainsKey(gameCode))
            {
                doubleTurns[gameCode] = new List<string>();
            }
            if (!doubleTurns[gameCode].Contains(gamertag))
            {
                doubleTurns[gameCode].Add(gamertag);
            }
        }

        public void AddPlayersToGame(List<string> playerGamertags, string gameCode)
        {
            int userId = -1;
            int gameId = GetGameId(gameCode);
            foreach (var playerGamertag in playerGamertags)
            {
                if (playerGamertag.StartsWith("GUEST"))
                {
                    int guestId = int.Parse(playerGamertag.Substring(6));
                    userId = GuestManagerDB.GetUserIdByGuestId(guestId);
                }
                else
                {
                    var player = PlayerManagerDB.GetPlayerByGamertag(playerGamertag);
                    userId = player.userId;
                }
                PlayersByGameManagerDB.AddPlayerToGame(gameId, userId);
            }
        }

        public List<int> GetGamePlayers(int gameId)
        {
            return PlayersByGameManagerDB.GetGamePlayersId(gameId);
        }
        public List<int> GetAccountsIdByPlayerId(List<int> playersId)
        {
            return PlayersByGameManagerDB.GetAccountsIdByPlayerId(playersId);
        }

        public int GetGameId(string gameCode)
        {
            return GameManagerDB.GetGameId(gameCode);
        }

        public void DisconnectGame(string gameCode, string gamertag)
        {
            if (gameConnections.ContainsKey(gameCode) && gameConnections[gameCode].ContainsKey(gamertag))
            {
                if (gameConnections[gameCode].Count == 1)
                {
                    GameManagerDB.DeleteGame(gameCode);
                    Console.WriteLine("Juego terminado por falta de jugadores");
                }
                RemovePlayerByGame(gameCode, gamertag);
                gameConnections[gameCode].Remove(gamertag);
                playerStatus[gameCode].Remove(gamertag);
                Console.WriteLine($"{gamertag} se ha desconectado");
                
            }
        }
    }
}
