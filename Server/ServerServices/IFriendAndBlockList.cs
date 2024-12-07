using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerServices
{
    public interface IFriendAndBlockList
    {
        int AddFriend(Friend friend);
        bool RemoveFriend(Friend friend);
        Dictionary<int, string> GetFriendsGamertags(int playerId);
        int AddBlock(BlockList blockList);
        bool RemoveBlock(BlockList blockList);
        Dictionary<int, string> GetBlockedGamertags(int playerId);
        bool IsPlayerBlocked(BlockList blockList);
}
