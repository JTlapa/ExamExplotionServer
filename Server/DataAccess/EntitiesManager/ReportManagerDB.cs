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
    public class ReportManagerDB
    {
        public static int GetReportCount(int playerId)
        {
            int reports;
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
                // Log de error SQL
                reports = -1;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
                reports = -1;
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
                reports = -1;
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
                reports = -1;
            }
            return reports;
        }

        public static bool ReportPlayer(int reportedPlayerId)
        {
            Report reportObtained;
            bool reported;
            try
            {
                using(var context = new ExamExplotionDBEntities())
                {
                    reportObtained = context.Report.FirstOrDefault(report => report.idPlayerReported == reportedPlayerId);
                    reportObtained.amount += 1;
                    reported = reportObtained.amount > 0;
                }
            }
            catch (SqlException sqlException)
            {
                // Log de error SQL
                reported = false;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                // Log de error de operación inválida
                reported = false;
            }
            catch (EntityException entityException)
            {
                // Log de error de Entity Framework
                reported = false;
            }
            catch (Exception ex)
            {
                // Log de cualquier otro error no especificado
                reported = false;
            }
            return reported;
        }
    }
}
