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

        [OperationContract]
        List<string> Connect(string user, string lobbyCode);

        [OperationContract]
        void Disconnect(string lobbyCode, string gamertag);

        [OperationContract]
        string CreateLobby(GameM gameReceived);

        [OperationContract]
        bool JoinLobby(string code, string gamertag);

    }
    public interface ILobbyConnectionCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string gamertag, string message);
        [OperationContract(IsOneWay = true)]
        void OnPlayerJoined(string gamertag);
        [OperationContract(IsOneWay = true)]
        void OnPlayerLeft(string gamertag);
    }
}
