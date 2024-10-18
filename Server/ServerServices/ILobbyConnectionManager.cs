using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerServices
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    interface ILobbyConnectionManager
    {
        [OperationContract]
        bool JoinLobby(string code, string gamertag, int maxPlayers);
    }
}
