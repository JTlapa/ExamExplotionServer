using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    /// <summary>
    /// Define las operaciones relacionadas con la administración de jugadores, incluyendo registro, puntuaciones, estadísticas y listas de amigos.
    /// </summary>
    [ServiceContract]
    interface IPlayerManager
    {
        /// <summary>
        /// Registra un nuevo jugador en el sistema.
        /// </summary>
        /// <param name="player">Objeto <see cref="PlayerManagement"/> con los datos del jugador a registrar.</param>
        /// <returns>Verdadero si el registro fue exitoso; falso en caso contrario.</returns>
        [OperationContract]
        bool RegisterPlayer(PlayerManagement player);

        /// <summary>
        /// Actualiza la puntuación de un jugador.
        /// </summary>
        /// <param name="userId">Identificador único del jugador.</param>
        /// <param name="newScore">Nueva puntuación del jugador.</param>
        /// <returns>Verdadero si la puntuación fue actualizada correctamente; falso en caso contrario.</returns>
        [OperationContract]
        bool UpdateScore(int userId, int newScore);

        /// <summary>
        /// Obtiene el número de victorias de un jugador.
        /// </summary>
        /// <param name="playerId">Identificador único del jugador.</param>
        /// <returns>Cantidad de victorias del jugador.</returns>
        [OperationContract]
        int GetWins(int playerId);

        /// <summary>
        /// Obtiene los puntos actuales de un jugador.
        /// </summary>
        /// <param name="playerId">Identificador único del jugador.</param>
        /// <returns>Puntuación actual del jugador.</returns>
        [OperationContract]
        int GetPoints(int playerId);

        /// <summary>
        /// Agrega a otro jugador como amigo en la lista de amigos del jugador especificado.
        /// </summary>
        /// <param name="playerId">Identificador único del jugador que agrega al amigo.</param>
        /// <param name="friendId">Identificador único del jugador a agregar como amigo.</param>
        /// <returns>Verdadero si el amigo fue agregado exitosamente; falso en caso contrario.</returns>
        [OperationContract]
        bool AddFriend(int playerId, int friendId);

        /// <summary>
        /// Agrega un jugador invitado al sistema.
        /// </summary>
        /// <returns>Un objeto <see cref="GuestManagement"/> con los detalles del jugador invitado creado.</returns>
        [OperationContract]
        GuestManagement AddGuest();

        /// <summary>
        /// Obtiene los detalles de un jugador a partir de su gamertag.
        /// </summary>
        /// <param name="gamertag">Gamertag del jugador.</param>
        /// <returns>Un objeto <see cref="PlayerManagement"/> con los datos del jugador.</returns>
        [OperationContract]
        PlayerManagement GetPlayerByGamertag(string gamertag);

        /// <summary>
        /// Obtiene el leaderboard global, ordenado por puntuación.
        /// </summary>
        /// <returns>Un diccionario donde las claves son los gamertags de los jugadores y los valores son sus puntuaciones.</returns>
        [OperationContract]
        Dictionary<string, int> GetGlobalLeaderboard();

        /// <summary>
        /// Obtiene el leaderboard de amigos de un jugador.
        /// </summary>
        /// <param name="playerId">Identificador único del jugador.</param>
        /// <returns>Un diccionario donde las claves son los gamertags de los amigos y los valores son sus puntuaciones.</returns>
        [OperationContract]
        Dictionary<string, int> GetFriendsLeaderboard(int playerId);
    }

    /// <summary>
    /// Representa la información de un jugador registrado en el sistema.
    /// </summary>
    [DataContract]
    public class PlayerManagement
    {
        private int userId;
        private int accountId;
        private int score;
        private int wins;

        /// <summary>
        /// Identificador único del jugador.
        /// </summary>
        [DataMember]
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Identificador único de la cuenta asociada al jugador.
        /// </summary>
        [DataMember]
        public int AccountId { get { return accountId; } set { accountId = value; } }

        /// <summary>
        /// Puntuación acumulada del jugador.
        /// </summary>
        [DataMember]
        public int Score { get { return score; } set { score = value; } }

        /// <summary>
        /// Cantidad de victorias obtenidas por el jugador.
        /// </summary>
        [DataMember]
        public int Wins { get { return wins; } set { wins = value; } }
    }

    /// <summary>
    /// Representa la información de un jugador invitado.
    /// </summary>
    [DataContract]
    public class GuestManagement
    {
        private int userId;
        private int guestNumber;

        /// <summary>
        /// Identificador único del jugador invitado.
        /// </summary>
        [DataMember]
        public int UserId { get { return userId; } set { userId = value; } }

        /// <summary>
        /// Número único asociado al jugador invitado.
        /// </summary>
        [DataMember]
        public int GuestNumber { get { return guestNumber; } set { guestNumber = value; } }
    }
}
