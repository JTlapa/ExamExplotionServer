using DataAccess;
using DataAccess.EntitiesManager;
using ServerServices;

namespace ServerService
{
    public partial class ServiceImplementation : IAccountManager
    {
        public bool validateLogIn(AccountM account)
        {
            Account accountToValidate = new Account();
            accountToValidate.gamertag = account.Gamertag;
            accountToValidate.password = account.Password;

            bool result = AccountManagerDB.ValidateAccount(accountToValidate);

            return result;
        }
    }

    public partial class ServiceImplementation : IPlayerManager
    {
        bool IPlayerManager.AddFriend(int playerId, int friendId)
        {
            throw new System.NotImplementedException();
        }

        int IPlayerManager.GetWins(int playerId)
        {
            throw new System.NotImplementedException();
        }

        bool IPlayerManager.RegisterPlayer(PlayerM player)
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
    }
}
