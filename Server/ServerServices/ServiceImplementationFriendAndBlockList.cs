using DataAccess;
using DataAccess.EntitiesManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    public partial class ServiceImplementation : IFriendAndBlockList
    {
        public int AddBlock(BlockListManagement blockList)
        {
            BlockList block = new BlockList();
            block.idPlayer = blockList.PlayerId;
            block.blockedPlayer = blockList.BlockedPlayerId;
            return BlockListManagerDB.AddBlock(block);
        }

        public int AddFriend(FriendManagement friend)
        {
            Friend friendToAdd = new Friend();
            friendToAdd.playerId1 = friend.Player1Id;
            friendToAdd.playerId2 = friend.Player2Id;
            return FriendManagerDB.AddFriend(friendToAdd);
        }

        public Dictionary<int, string> GetBlockedGamertags(int playerId)
        {
            return BlockListManagerDB.GetGamertagsBlocked(playerId);
        }

        public Dictionary<int, string> GetFriendsGamertags(int playerId)
        {
            return FriendManagerDB.GetFriendsGamertags(playerId);
        }

        public bool RemoveBlock(BlockListManagement blockList)
        {
            BlockList block = new BlockList();
            block.idPlayer = blockList.PlayerId;
            block.blockedPlayer = blockList.BlockedPlayerId;
            return BlockListManagerDB.RemoveBlockList(block);
        }

        public bool RemoveFriend(FriendManagement friend)
        {
            Friend friendToRemove = new Friend();
            friendToRemove.playerId1 = friend.Player1Id;
            friendToRemove.playerId2 = friend.Player2Id;
            return FriendManagerDB.RemoveFriend(friendToRemove);
        }
    }
}
