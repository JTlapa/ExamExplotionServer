using DataAccess;
using DataAccess.EntitiesManager;
using ServerServices;
using System;
using System.Windows;
using System.Collections.Generic;
using System.ServiceModel;
using System.Resources;
using System.Linq;

namespace ServerService
{
    public partial class ServiceImplementation : ILobbyManager
    {
        private static readonly Dictionary<string, Dictionary<string, ILobbyConnectionCallback>> lobbyConnections = new Dictionary<string, Dictionary<string, ILobbyConnectionCallback>>();

        public bool Connect(string gamertag, string lobbyCode)
        {
            var callback = OperationContext.Current.GetCallbackChannel<ILobbyConnectionCallback>();
            bool connected = false;
            if (!lobbyConnections[lobbyCode].ContainsKey(gamertag))
            {
                lobbyConnections[lobbyCode].Add(gamertag, callback);
                connected = true;
                Console.WriteLine($"{gamertag} conectado.");
            }
            return connected;
        }

        public void Disconnect(string gamertag)
        {
            /*if (connectedUsers.ContainsKey(gamertag))
            {
                connectedUsers.Remove(gamertag);
                Console.WriteLine($"{gamertag} desconectado.");
            }*/
        }

        public string CreateLobby(GameM gameReceived)
        {
            string code = null;
            do
            {
                code = GenerateLobbyCode();
            }
            while (lobbyConnections.ContainsKey(code));

            Game gameToRegister = new Game();
            gameToRegister.timePerTurn = gameReceived.TimePerTurn;
            gameToRegister.invitationCode = code;
            gameToRegister.lives = gameReceived.Lives;
            gameToRegister.numberPlayers = gameReceived.NumberPlayers;
            gameToRegister.hostPlayerId = gameReceived.HostPlayerId;
            return code;
        }
        private static string GenerateLobbyCode()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            return new string(Enumerable.Repeat(chars, 4).Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public void SendMessage(string code, string gamertag, string message)
        {
            if (lobbyConnections.ContainsKey(code))
            {
                var playersInLobby = lobbyConnections[code];

                foreach (var player in playersInLobby)
                {
                    player.Value.ReceiveMessage(gamertag, message);
                }
            }
        }
    }

    public partial class ServiceImplementation : ILobbyConnectionManager
    {
        public bool JoinLobby(string code, string player, int maxPlayers)
        {
            bool joined = false;
            if (lobbyConnections.ContainsKey(code))
            {
                var players = lobbyConnections[code];

                if (!players.ContainsKey(player))
                {
                    if (players.Count < maxPlayers)
                    {
                        joined = true;
                    }
                }
            }
            return joined;
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
