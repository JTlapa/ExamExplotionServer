using DataAccess.EntitiesManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    public partial class ServiceImplementation : IReportManager
    {
        public int GetReportCount(int playerId)
        {
            return ReportManagerDB.GetReportCount(playerId);
        }

        public bool ReportPlayer(int reportedPlayerId)
        {
            return ReportManagerDB.ReportPlayer(reportedPlayerId);
        }
    }
}
