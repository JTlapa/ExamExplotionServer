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
    interface IPlayerManager
    {
        [OperationContract]
        bool RegisterPlayer(PlayerM player);

        [OperationContract]
        bool UpdateScore(int userId, int newScore);

        [OperationContract]
        int GetWins(int playerId);

        [OperationContract]
        bool AddFriend(int playerId, int friendId);
    }

    [DataContract]
    public class PlayerM
    {
        private int userId;
        private int accountId;
        private int score;
        private int wins;

        [DataMember]
        public int UserId { get { return userId; } set { userId = value; } }
        [DataMember]
        public int AccountId { get { return accountId; } set { accountId = value; } }
        [DataMember]
        public int Score { get { return score; } set { score = value; } }
        [DataMember]
        public int Wins { get { return wins; } set { wins = value; } }
    }

}
