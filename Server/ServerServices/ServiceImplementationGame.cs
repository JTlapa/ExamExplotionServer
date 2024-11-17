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
        private Dictionary<string, Stack<Card>> gameDeck = new Dictionary<string, Stack<Card>>();

        public void InitializeDeck(string gameCode, int playerCount)
        {
            List<Card> cards = new List<Card>();
            int repiteCount = Math.Max(0, playerCount - 1);
            int reinscripcionCount = Math.Max(0, 4 -  playerCount);
            AddCardToList(cards, "Repite", repiteCount);
            AddCardToList(cards, "Reinscripcion", reinscripcionCount);
            AddCardToList(cards, "Ver el futuro", 6);
            AddCardToList(cards, "Dejo el equipo", 6);
            AddCardToList(cards, "Exentar", 5);
            AddCardToList(cards, "Paro", 6);
            AddCardToList(cards, "Revolver", 6);
            AddCardToList(cards, "Agarrar de abajo", 5);
            AddCardToList(cards, "El profe R", 3);
            AddCardToList(cards, "El profe O", 3);
            AddCardToList(cards, "El profe S", 3);
            AddCardToList(cards, "El profe A", 3);
            AddCardToList(cards, "El profe M", 3);

            int remainingCards = 56 - cards.Count;
            if (remainingCards > 0)
            {
                AddCardToList(cards, "Agarrar de abajo", remainingCards);
            }
            var shuffledDeck = cards.OrderBy(cardDeck => Guid.NewGuid()).ToList();
            gameDeck[gameCode] = new Stack<Card>(shuffledDeck);
            Console.WriteLine($"Deck inicializado, hay {cards.Count} cartas");
        }

        private void AddCardToList(List<Card> cardList, string cardName, int count)
        {
            for (int i = 0; i < count; i++)
            {
                cardList.Add(new Card { CardName = cardName });
            }
        }

        public Card DrawCard(string gameCode)
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

        public List<Card> SeeTheFuture(string gameCode)
        {
            if (gameDeck.ContainsKey(gameCode) && gameDeck[gameCode].Count > 0)
            {
                return gameDeck[gameCode].Take(3).ToList();
            }
            else
            {
                return new List<Card>();
            }
        }

        public bool AddCardToDeck(string gameCode, Card card)
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
                gameDeck[gameCode] = new Stack<Card>(cards);
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

        public string GetGameStatus(string gameCode)
        {
            throw new NotImplementedException();
        }

        public List<PlayerManagement> GetPlayersInGame(string gameCode)
        {
            throw new NotImplementedException();
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
    }
}
