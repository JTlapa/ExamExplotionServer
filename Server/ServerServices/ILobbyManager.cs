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
        bool Connect(string user, string lobbyCode);
        [OperationContract]
        void Disconnect(string gamertag);

        [OperationContract]
        string CreateLobby(GameM gameReceived);

    }
    public interface ILobbyConnectionCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string gamertag, string message);
    }
}
