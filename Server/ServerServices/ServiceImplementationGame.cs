using DataAccess;
using DataAccess.EntitiesManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    public partial class ServiceImplementation : IGameManager
    {
        private Dictionary<string, string> currentTurnByGame = new Dictionary<string, string>();
        private Dictionary<string, List<string>> playersInGame = new Dictionary<string, List<string>>();
        private static Dictionary<string, Dictionary<string, IGameConnectionCallback>> gameConnections = new Dictionary<string, Dictionary<string, IGameConnectionCallback>>();
        private static Dictionary<string, bool> turnTransitionState = new Dictionary<string, bool>();
        private Dictionary<string, Stack<CardManagement>> gameDeck = new Dictionary<string, Stack<CardManagement>>();
        private Dictionary<string, Dictionary<string, bool>> cardsDealt = new Dictionary<string, Dictionary<string, bool>>();  

        //Esta funcion no va a ir aqui
        public void InitializeDeck(string gameCode, int playerCount, string gamertag)
        {
            if (!gameDeck.ContainsKey(gameCode))
            {
                gameDeck.Add(gameCode, new Stack<CardManagement>());
            }
            if (!cardsDealt.ContainsKey(gameCode))
            {
                cardsDealt.Add(gameCode, new Dictionary<string, bool>());
            }
            cardsDealt[gameCode].Add(gamertag, false);
            if (gameDeck[gameCode].Count == 0)
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

                var shuffledDeck = cards.OrderBy(cardDeck => Guid.NewGuid()).ToList();
                gameDeck[gameCode] = new Stack<CardManagement>(shuffledDeck);
            }
        }
        //esta tampoco va aqui
        private void AddCardToList(List<CardManagement> cardList, string cardName, string cardPath, int count)
        {
            for (int i = 0; i < count; i++)
            {
                cardList.Add(new CardManagement { CardName = cardName, CardPath = cardPath});
            }
        }
        //tampoco iria
        public CardManagement DrawCard(string gameCode)
        {
            if (gameDeck.ContainsKey(gameCode) && gameDeck[gameCode].Count > 0)
            {
                return gameDeck[gameCode].Pop();
            }
            else
            {
                return null;
            }
        }

        public List<CardManagement> SeeTheFuture(string gameCode)
        {
            if (gameDeck.ContainsKey(gameCode) && gameDeck[gameCode].Count > 0)
            {
                return gameDeck[gameCode].Take(3).ToList();
            }
            else
            {
                return new List<CardManagement>();
            }
        }

        public bool AddCardToDeck(string gameCode, CardManagement card)
        {
            bool added = false;
            if (gameDeck.ContainsKey(gameCode))
            {
                gameDeck[gameCode].Push(card);
                added = true;
            }
            return added;
        }

        public bool ShuffleDeck(string gameCode)
        {
            bool shuffled = false;
            if (gameDeck.ContainsKey(gameCode))
            {
                var cards = gameDeck[gameCode].ToList();
                cards = cards.OrderBy(_ => Guid.NewGuid()).ToList();
                gameDeck[gameCode] = new Stack<CardManagement>(cards);
                shuffled = true;
            }
            return shuffled;
        }

        public bool EndGame(string gameCode, int winnerPlayerId)
        {
            throw new NotImplementedException();
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
            playersInGame[gameCode] = gamertags.OrderBy(_ => Guid.NewGuid()).ToList();
            var firstPlayer = playersInGame[gameCode].First();
            currentTurnByGame[gameCode] = firstPlayer;
            turnTransitionState[gameCode] = false;
            NotifyClientOfTurn(gameCode, firstPlayer);
        }

        public void NotifyClientOfTurn(string gameCode, string nextGametag)
        {
            if (gameConnections.ContainsKey(gameCode))
            {
                var playersInGame = gameConnections[gameCode];
                foreach (var player in playersInGame)
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

                    int nextIndex = (currentIndex + 1) % players.Count;
                    var nextGametag = players[nextIndex];

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
            return connected;
        }

        public Dictionary<string, int> GetPlayerDeck(string gameCode, string gamertag)
        {
            Dictionary<string, int> playerDeck = new Dictionary<string, int>();
            playerDeck.Add("reRegistration", 1);
            for(int i = 0; i < 7; i++)
            {
                CardManagement card = gameDeck[gameCode].Pop();
                if (playerDeck.ContainsKey(card.CardPath))
                {
                    playerDeck[card.CardPath] += 1;
                }
                else
                {
                    playerDeck.Add(card.CardPath, 1);
                }
            }
            cardsDealt[gameCode][gamertag] = true;
            FinalizeGameDeck(gameCode);
            return playerDeck;
        }

        private void FinalizeGameDeck(string gameCode)
        {
            bool cardsDealtReady = true;
            int playerCount = 0;
            foreach (bool player in cardsDealt[gameCode].Values)
            {
                if (!player)
                {
                    cardsDealtReady = false;
                }
                playerCount++;
            }
            if (cardsDealtReady)
            {
                int repiteCount = Math.Max(0, playerCount - 1);
                int reinscripcionCount = Math.Max(0, 6 - playerCount);

                List<CardManagement> cards = gameDeck[gameCode].ToList();
                AddCardToList(cards, "Repite", "examBomb", repiteCount);
                AddCardToList(cards, "Reinscripcion", "reRegistration", reinscripcionCount);
                var shuffledDeck = cards.OrderBy(cardDeck => Guid.NewGuid()).ToList();

                gameDeck[gameCode] = new Stack<CardManagement>(shuffledDeck);
                Console.WriteLine($"Quedaron {gameDeck[gameCode].Count} cartas despues de repartir");
            }
        }
    }
}
