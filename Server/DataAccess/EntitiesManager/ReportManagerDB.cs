using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public static class ReportManagerDB
    {

        private static readonly ILog log = LogManager.GetLogger(typeof(ReportManagerDB));
        public static int GetReportCount(int playerId)
        {
            int reports = -1;
            try
            {
                using(var context = new ExamExplotionDBEntities())
                {
                    reports = context.Report.Where(report => report.idPlayerReported == playerId)
                        .Select(report => report.amount)
                        .FirstOrDefault();
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            return reports;
        }

        public static bool ReportPlayer(int reportedPlayerId)
        {
            Report reportObtained;
            bool reported = false;
            try
            {
                using(var context = new ExamExplotionDBEntities())
                {
                    reportObtained = context.Report.FirstOrDefault(report => report.idPlayerReported == reportedPlayerId);
                    if(reportObtained == null)
                    {
                        reportObtained = AddReport(reportedPlayerId);
                    }
                    else
                    {
                        reportObtained.amount += 1;
                    }
                    reported = reportObtained.amount > 0;
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            return reported;
        }

        private static Report AddReport(int reportedPlayerId)
        {
            Report report = new Report();
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    report = context.Report.Add(new Report { amount = 1, idPlayerReported = reportedPlayerId });
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            return report;
        }
    }
}
