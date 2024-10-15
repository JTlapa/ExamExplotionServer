using DataAccess;
using DataAccess.EntitiesManager;
using ServerServices;
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
                connectedUsers[gamertag] = callback;
                Console.WriteLine($"{gamertag} conectado.");
                return true;
            }

            return false;
        }

        public void SendMessage(string gamertag, string message)
        {
            List<string> disconnectedClients = new List<string>();

            foreach (var client in connectedUsers)
            {
                try
                {
                    client.Value.ReceiveMessage(gamertag, message);
                }
                catch (CommunicationObjectAbortedException)
                {
                    disconnectedClients.Add(client.Key);
                    Console.WriteLine($"Cliente {client.Key} se ha desconectado.");
                }
            }

            foreach (var disconnectedClient in disconnectedClients)
            {
                connectedUsers.Remove(disconnectedClient);
            }
        }

    }

    public partial class ServiceImplementation : IPlayerManager
    {
        bool IPlayerManager.AddFriend(int playerId, int friendId)
        {
            throw new NotImplementedException();
        }

        int IPlayerManager.GetWins(int playerId)
        {
            return PlayerManagerDB.GetWins(playerId);
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
