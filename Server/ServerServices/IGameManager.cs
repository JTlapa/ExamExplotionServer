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
        [OperationContract]
        bool ConnectGame(string gameCode, string gamertag);
        [OperationContract]
        bool EndGame(string gameCode, int winnerPlayerId);
        [OperationContract(IsOneWay = true)]
        void NotifyEndTurn(string gameCode, string currentGamertag);
        [OperationContract]
        List<PlayerManagement> GetPlayersInGame(string gameCode);
        [OperationContract]
        string GetGameStatus(string gameCode);
        [OperationContract]
        string GetCurrentTurn(string gameCode);
        [OperationContract]
        GameManagement GetGame(string gameCode);
        [OperationContract(IsOneWay = true)]
        void InitializeGameTurns(string gameCode, List<string> gamertags);
        [OperationContract(IsOneWay = true)]
        void NotifyClientOfTurn(string gameCode, string nextGametag);
    }

    public interface IGameConnectionCallback
    {
        [OperationContract(IsOneWay = true)]
        void UpdateCurrentTurn(string gamertag);
        [OperationContract(IsOneWay = true)]
        void SyncTimer();
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
