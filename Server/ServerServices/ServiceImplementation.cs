using DataAccess;
using DataAccess.EntitiesManager;
<<<<<<< HEAD
using ServerServices;
=======
using System;
using System.Collections.Generic;
using System.ServiceModel;
>>>>>>> 754b0ba372397191d518c9a129b79cc74768fad3

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
    public partial class ServiceImplementation : ILobbyManager
    {
        private static Dictionary<string, ILobbyConnectionCallback> connectedUsers = new Dictionary<string, ILobbyConnectionCallback>();

        public bool Connect(string gamertag)
        {
            var callback = OperationContext.Current.GetCallbackChannel<ILobbyConnectionCallback>();

            if (!connectedUsers.ContainsKey(gamertag))
            {
                connectedUsers.Add(gamertag, callback);
                Console.WriteLine($"{gamertag} conectado.");
                return true;
            }

            return false;
        }

        public void SendMessage(string gamertag, string message)
        {
            foreach (var client in connectedUsers.Values)
            {
                client.ReceiveMessage(gamertag, message);
            }
>>>>>>> 754b0ba372397191d518c9a129b79cc74768fad3
        }
    }
}
