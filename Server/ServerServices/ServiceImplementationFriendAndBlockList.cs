using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerServices
{
    public class ServiceImplementationFriendAndBlockList : IFriendAndBlockList
    {
        public int AddBlock(BlockList blockList)
        {
            throw new NotImplementedException();
        }

        public int AddFriend(Friend friend)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, string> GetBlockedGamertags(int playerId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<int, string> GetFriendsGamertags(int playerId)
        {
            throw new NotImplementedException();
        }

        public bool IsPlayerBlocked(BlockList blockList)
        {
            throw new NotImplementedException();
        }

        public bool RemoveBlock(BlockList blockList)
        {
            throw new NotImplementedException();
        }

        public bool RemoveFriend(Friend friend)
        {
            throw new NotImplementedException();
        }
    }
}
