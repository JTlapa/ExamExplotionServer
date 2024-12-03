using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    [ServiceContract(CallbackContract = typeof(IGameConnectionCallback))]
    public interface IGameManager
    {
        [OperationContract(IsOneWay = true)]
        void InitializeDeck(string gameCode, int playerCount, string gamertag);
        [OperationContract]
        bool ConnectGame(string gameCode, string gamertag);
        [OperationContract]
        bool EndGame(string gameCode, int winnerPlayerId);
        [OperationContract(IsOneWay = true)]
        void NotifyEndTurn(string gameCode, string currentGamertag);
        [OperationContract]
        GameManagement GetGame(string gameCode);
        [OperationContract(IsOneWay = true)]
        void InitializeGameTurns(string gameCode, List<string> gamertags);
        [OperationContract(IsOneWay = true)]
        void NotifyClientOfTurn(string gameCode, string nextGametag);
        [OperationContract (IsOneWay = true)]
        void NotifyDrawCard(string gameCode, string gamertag, bool isTopCard);
        [OperationContract (IsOneWay = true)]
        void NotifyCardOnBoard(string gameCode, string path);
     }

    public interface IGameConnectionCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateCurrentTurn(string gamertag);
        [OperationContract(IsOneWay = true)]
        void SyncTimer();
        [OperationContract(IsOneWay = true)]
        void RecivePlayerAndGameDeck(Stack<CardManagement> gameDeck, List<CardManagement> playerDeck);
        [OperationContract(IsOneWay = true)]
        void RemoveCardFromStack(bool isTopCard);
        [OperationContract(IsOneWay = true)]
        void PrintCardOnBoard(string path);
    }

    [DataContract]
    public class CardManagement
    {
        private string cardName;
        private string cardPath;
        [DataMember]
        public string CardName { get { return cardName; } set { cardName = value; } }
        [DataMember]
        public string CardPath { get { return cardPath; } set { cardPath = value; } }
    }

    [DataContract]
    public class GameManagement
    {
        private int gameId;
        private string invitationCode;
        private string gameStatus;
        private int numberPlayers;
        private int timePerTurn;
        private int lives;
        private int hostPlayerId;
        private int winnerPlayerId;

        [DataMember]
        public int GameId { get { return gameId; } set { gameId = value; } }

        [DataMember]
        public string InvitationCode { get { return invitationCode; } set { invitationCode = value; } }

        [DataMember]
        public string GameStatus { get { return gameStatus; } set { gameStatus = value; } }

        [DataMember]
        public int NumberPlayers { get { return numberPlayers; } set { numberPlayers = value; } }

        [DataMember]
        public int TimePerTurn { get { return timePerTurn; } set { timePerTurn = value; } }

        [DataMember]
        public int Lives { get { return lives; } set { lives = value; } }

        [DataMember]
        public int HostPlayerId { get { return hostPlayerId; } set { hostPlayerId = value; } }

        [DataMember]
        public int WinnerPlayerId { get { return winnerPlayerId; } set { winnerPlayerId = value; } }
    }
}
