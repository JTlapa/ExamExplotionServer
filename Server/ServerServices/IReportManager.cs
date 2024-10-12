using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerServices
{
    [ServiceContract]
    interface IReportManager
    {
        [OperationContract]
        bool ReportPlayer(int reportedPlayerId);

        [OperationContract]
        int GetReportCount(int playerId);
    }

    [DataContract]
    public class ReportM
    {
        private int reportedPlayerId;
        private int reportAmount;

        [DataMember]
        public int ReportedPlayerId { get { return reportedPlayerId; } set { reportedPlayerId = value; } }
        [DataMember]
        public int ReportAmount { get { return reportAmount; } set { reportAmount = value; } }
    }
}
