using ServerServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    [ServiceContract(CallbackContract = typeof(ILobbyConnectionCallback))]
    public interface ILobbyManager
    {
        [OperationContract(IsOneWay = true)]
        void SendMessage(string gameCode,string gamertag, string message);

        [OperationContract(IsOneWay = true)]
        void Connect(string user, string lobbyCode);

        [OperationContract]
        void Disconnect(string lobbyCode, string gamertag);

        [OperationContract]
        string CreateLobby(GameManagement gameReceived);

        [OperationContract]
        bool JoinLobby(string code, string gamertag);

        [OperationContract(IsOneWay = true)]
        void UpdatePlayerStatus(string code, string gamertag, bool isReady);
        [OperationContract]
        void LeaveLobby(string code, string gamertag);
        [OperationContract(IsOneWay = true)]
        void PlayGame(string lobbyCode);
    }
    public interface ILobbyConnectionCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string gamertag, string message);
        [OperationContract(IsOneWay = true)]
        void Repaint(Dictionary<string, bool> playerStatus);
        [OperationContract(IsOneWay = true)]
        void StartGame(Dictionary<string, bool> lobbyPlayers);
    }
}
