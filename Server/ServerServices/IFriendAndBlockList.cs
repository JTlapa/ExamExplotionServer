using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    [ServiceContract]
    public interface IFriendAndBlockList
    {
        [OperationContract]
        int AddFriend(FriendManagement friend);
        [OperationContract]
        bool RemoveFriend(FriendManagement friend);
        [OperationContract]
        Dictionary<int, string> GetFriendsGamertags(int playerId);
        [OperationContract]
        int AddBlock(BlockListManagement blockList);
        [OperationContract]
        bool RemoveBlock(BlockListManagement blockList);
        [OperationContract]
        Dictionary<int, string> GetBlockedGamertags(int playerId);
    }
    [DataContract]
    public class FriendManagement
    {
        private int player1Id;
        private int player2Id;

        [DataMember]
        public int Player1Id
        {
            get { return player1Id; }
            set { player1Id = value; }
        }
        [DataMember]
        public int Player2Id
        { 
            get { return player2Id; } 
            set { player2Id = value; } 
        }

    }
    [DataContract]
    public class BlockListManagement
    {
        private int playerId;
        private int blockedPlayerId;

        [DataMember]
        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }
        [DataMember]
        public int BlockedPlayerId
        {
            get { return blockedPlayerId; }
            set { blockedPlayerId = value; }
        }

    }
}
