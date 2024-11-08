using DataAccess;
using DataAccess.EntitiesManager;
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
        private static Dictionary<string, Dictionary<string, ILobbyConnectionCallback>> lobbyConnections = new Dictionary<string, Dictionary<string, ILobbyConnectionCallback>>();
        private static Dictionary<string, Dictionary<string, bool>> playerStatus = new Dictionary<string, Dictionary<string, bool>>();

        public void Connect(string gamertag, string lobbyCode)
        {
            var callback = OperationContext.Current.GetCallbackChannel<ILobbyConnectionCallback>();

            if (!lobbyConnections.ContainsKey(lobbyCode))
            {
                lobbyConnections[lobbyCode] = new Dictionary<string, ILobbyConnectionCallback>();
            }
            if (!playerStatus.ContainsKey(lobbyCode))
            {
                playerStatus[lobbyCode] = new Dictionary<string, bool>();
            }
            if (!lobbyConnections[lobbyCode].ContainsKey(gamertag))
            {
                lobbyConnections[lobbyCode].Add(gamertag, callback);
                if (!playerStatus[lobbyCode].ContainsKey(gamertag))
                {
                    playerStatus[lobbyCode].Add(gamertag, false);
                    Console.WriteLine($"{gamertag} conectado.");
                    NotifyPlayers(gamertag, lobbyCode);
                }
            }
        }

        private void NotifyPlayers(string gamertag, string lobbyCode)
        {
            foreach (var player in lobbyConnections[lobbyCode])
            {
                player.Value.Repaint(playerStatus[lobbyCode]);
            }
        }


        public void Disconnect(string lobbyCode, string gamertag)
        {
            if(lobbyConnections.ContainsKey(lobbyCode) && lobbyConnections[lobbyCode].ContainsKey(gamertag))
            {
                if (lobbyConnections[lobbyCode].Count == 1)
                {
                    GameManagerDB.DeleteGame(lobbyCode);
                    Console.WriteLine("Lobby eliminada");
                }
                lobbyConnections[lobbyCode].Remove(gamertag);
                playerStatus[lobbyCode].Remove(gamertag);
                Console.WriteLine($"{gamertag} se ha desconectado");

                foreach (var player in lobbyConnections[lobbyCode])
                {
                    player.Value.Repaint(playerStatus[lobbyCode]);
                }
            }
        }

        public string CreateLobby(GameManagement gameReceived)
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

            GameManagerDB.AddGame(gameToRegister);
            lobbyConnections.Add(code, new Dictionary<string, ILobbyConnectionCallback>());

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
        public bool JoinLobby(string code, string player)
        {
            bool joined = false;
            if (lobbyConnections.ContainsKey(code))
            {
                Game game = GameManagerDB.getGameByCode(code);
                int maxPlayers = game.numberPlayers ?? 0;
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

        public void UpdatePlayerStatus(string lobbyCode, string gamertag, bool isReady)
        {
            if (lobbyConnections.ContainsKey(lobbyCode) && lobbyConnections[lobbyCode].ContainsKey(gamertag))
            {
                playerStatus[lobbyCode][gamertag] = isReady;
                foreach (var player in lobbyConnections[lobbyCode])
                {
                    player.Value.Repaint(playerStatus[lobbyCode]);
                }
            }
        }
        public void LeaveLobby(string code, string gamertag)
        {
            if (lobbyConnections.ContainsKey(code))
            {
                if (lobbyConnections[code].ContainsKey(gamertag) && playerStatus[code].ContainsKey(gamertag))
                {
                    lobbyConnections[code].Remove(gamertag);
                    playerStatus[code].Remove(gamertag);
                    foreach (var player in lobbyConnections[code])
                    {
                        player.Value.Repaint(playerStatus[code]);
                    }
                }
            }
        }

        public void PlayGame(string lobbyCode)
        {
            foreach (var player in lobbyConnections[lobbyCode])
            {
                player.Value.StartGame(playerStatus[lobbyCode]);
            }
        }
    }
}
