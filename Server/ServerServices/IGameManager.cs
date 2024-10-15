using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerServices
{
    [ServiceContract]
    interface IGameManager
    {
        [OperationContract]
        int CreateGame(GameM game);

        [OperationContract]
        bool JoinGame(int gameId, int playerId);

        [OperationContract]
        bool StartGame(int gameId);

        [OperationContract]
        bool EndGame(int gameId, int winnerPlayerId);

        [OperationContract]
        List<PlayerM> GetPlayersInGame(int gameId);

        [OperationContract]
        string GetGameStatus(int gameId);
    }

    [DataContract]
    public class GameM
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
