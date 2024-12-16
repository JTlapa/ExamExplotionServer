using DataAccess.EntitiesManager;
using DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    public partial class ServiceImplementation : IPlayerManager
    {
        public GuestManagement AddGuest()
        {
            Guest guest = new Guest();
            Users user = new Users();
            int newUserId = UserManagerDB.AddUser(user);
            guest.userId = newUserId;
            int guestNumber = GuestManagerDB.AddGuest(guest);

            GuestManagement guestManagement = null;
            if(newUserId != -1 && guestNumber != -1)
            {
                guestManagement = new GuestManagement();
                guestManagement.GuestNumber = guestNumber;
                guestManagement.UserId = newUserId;
            }
            return guestManagement;
        }

        public Dictionary<string, int> GetFriendsLeaderboard(int playerId)
        {
            List<int> friendsIds = FriendManagerDB.GetFriendsIdByPlayer(playerId);
            friendsIds.Add(playerId);
            return PlayerManagerDB.GetLeaderboardByFriends(friendsIds);
        }

        public Dictionary<string, int> GetGlobalLeaderboard()
        {
            return PlayerManagerDB.GetGlobalLeaderboard();
        }

        public PlayerManagement GetPlayerByGamertag(string gamertag)
        {
            PlayerManagement playerManagement = new PlayerManagement();
            Player player = PlayerManagerDB.GetPlayerByGamertag(gamertag);

            playerManagement.UserId = player.userId;
            playerManagement.AccountId = player.accountId;

            return playerManagement;
        }

        public int GetPoints(int playerId)
        {
            return PlayerManagerDB.GetPoints(playerId);
        }

        int IPlayerManager.GetWins(int playerId)
        {
            return PlayerManagerDB.GetWins(playerId);
        }

        bool IPlayerManager.RegisterPlayer(PlayerManagement player)
        {
            Player playerToRegistrate = new Player();
            playerToRegistrate.userId = player.UserId;
            playerToRegistrate.wins = 0;
            playerToRegistrate.score = 0;
            playerToRegistrate.accountId = player.AccountId;

            bool playerRegistered = PlayerManagerDB.RegisterPlayer(playerToRegistrate);
            return playerRegistered;
        }

        bool IPlayerManager.UpdateScore(int userId, int newScore)
        {
            return PlayerManagerDB.UpdateScore(userId, newScore);
        }

        bool IPlayerManager.AddWin(int userId)
        {
            return PlayerManagerDB.AddWin(userId);
        }
    }
}
