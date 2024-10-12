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
        void SendMessage(string user, string message);

        [OperationContract]
        bool Connect(string user);

    }
    public interface ILobbyConnectionCallback
    {
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string gamertag, string message);
    }
}
