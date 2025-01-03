using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

<<<<<<< HEAD
namespace ServerServices
{
    [ServiceContract]
    interface IReportManager
    {
        [OperationContract]
        bool ReportPlayer(int reportedPlayerId);

=======
namespace ServerService
{
    /// <summary>
    /// Define el contrato de servicio para la gestión de reportes de jugadores.
    /// </summary>
    [ServiceContract]
    public interface IReportManager
    {
        /// <summary>
        /// Reporta a un jugador específico.
        /// </summary>
        /// <param name="reportedPlayerId">ID único del jugador que está siendo reportado.</param>
        /// <returns>Verdadero si el reporte se registró con éxito, falso en caso contrario.</returns>
        [OperationContract]
        bool ReportPlayer(int reportedPlayerId);

        /// <summary>
        /// Obtiene la cantidad total de reportes de un jugador.
        /// </summary>
        /// <param name="playerId">ID único del jugador para el cual se desea consultar el número de reportes.</param>
        /// <returns>Cantidad de reportes asociados al jugador especificado.</returns>
>>>>>>> f796509853e9a2b1f8b30832f75c8529577581bf
        [OperationContract]
        int GetReportCount(int playerId);
    }

<<<<<<< HEAD
    [DataContract]
    public class ReportM
=======
    /// <summary>
    /// Representa la estructura de datos para gestionar la información de reportes de jugadores.
    /// </summary>
    [DataContract]
    public class ReportManagement
>>>>>>> f796509853e9a2b1f8b30832f75c8529577581bf
    {
        private int reportedPlayerId;
        private int reportAmount;

<<<<<<< HEAD
        [DataMember]
        public int ReportedPlayerId { get { return reportedPlayerId; } set { reportedPlayerId = value; } }
        [DataMember]
        public int ReportAmount { get { return reportAmount; } set { reportAmount = value; } }
=======
        /// <summary>
        /// Obtiene o establece el ID del jugador reportado.
        /// </summary>
        [DataMember]
        public int ReportedPlayerId
        {
            get { return reportedPlayerId; }
            set { reportedPlayerId = value; }
        }

        /// <summary>
        /// Obtiene o establece la cantidad de reportes acumulados para el jugador.
        /// </summary>
        [DataMember]
        public int ReportAmount
        {
            get { return reportAmount; }
            set { reportAmount = value; }
        }
>>>>>>> f796509853e9a2b1f8b30832f75c8529577581bf
    }
}
