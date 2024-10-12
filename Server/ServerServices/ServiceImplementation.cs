using DataAccess;
using DataAccess.EntitiesManager;
using System;
using System.Collections.Generic;
using System.ServiceModel;

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
        }
    }
}
