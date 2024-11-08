using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class ReportManagerDB
    {
        public static int GetReportCount(int playerId)
        {
            using(var context = new ExamExplotionDBEntities())
            {
                int reports = context.Report.Where(report => report.idPlayerReported == playerId)
                    .Select(report => report.amount)
                    .FirstOrDefault();
                return reports;
            }
        }

        public static bool ReportPlayer(int reportedPlayerId)
        {
            using(var context = new ExamExplotionDBEntities())
            {
                var reportObtained = context.Report.FirstOrDefault(report => report.idPlayerReported == reportedPlayerId);
                reportObtained.amount += 1;
                return reportObtained.amount > 0;
            }
        }
    }
}
