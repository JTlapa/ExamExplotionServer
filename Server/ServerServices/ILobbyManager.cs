using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    /// <summary>
    /// Define las operaciones para la gestión de lobbies en un juego multijugador.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(ILobbyConnectionCallback))]
    public interface ILobbyManager
    {
        /// <summary>
        /// Envía un mensaje dentro de un lobby.
        /// </summary>
        /// <param name="gameCode">Código único del juego.</param>
        /// <param name="gamertag">Gamertag del jugador que envía el mensaje.</param>
        /// <param name="message">Contenido del mensaje a enviar.</param>
        [OperationContract(IsOneWay = true)]
        void SendMessage(string gameCode, string gamertag, string message);

        /// <summary>
        /// Conecta a un jugador a un lobby específico.
        /// </summary>
        /// <param name="user">Nombre del usuario que se conecta.</param>
        /// <param name="lobbyCode">Código único del lobby al que se conecta el jugador.</param>
        [OperationContract(IsOneWay = true)]
        void Connect(string user, string lobbyCode);

        /// <summary>
        /// Desconecta a un jugador de un lobby.
        /// </summary>
        /// <param name="lobbyCode">Código único del lobby del que se desconecta el jugador.</param>
        /// <param name="gamertag">Gamertag del jugador que se desconecta.</param>
        [OperationContract]
        void Disconnect(string lobbyCode, string gamertag);

        /// <summary>
        /// Crea un nuevo lobby para un juego.
        /// </summary>
        /// <param name="gameReceived">Objeto <see cref="GameManagement"/> que contiene los detalles del juego.</param>
        /// <returns>El código único del lobby creado.</returns>
        [OperationContract]
        string CreateLobby(GameManagement gameReceived);

        /// <summary>
        /// Permite a un jugador unirse a un lobby existente.
        /// </summary>
        /// <param name="code">Código único del lobby.</param>
        /// <param name="gamertag">Gamertag del jugador que se une.</param>
        /// <returns>Verdadero si el jugador se unió exitosamente; falso en caso contrario.</returns>
        [OperationContract]
        bool JoinLobby(string code, string gamertag);

        /// <summary>
        /// Actualiza el estado de preparación de un jugador dentro de un lobby.
        /// </summary>
        /// <param name="code">Código único del lobby.</param>
        /// <param name="gamertag">Gamertag del jugador cuyo estado se actualiza.</param>
        /// <param name="isReady">Verdadero si el jugador está listo; falso en caso contrario.</param>
        [OperationContract(IsOneWay = true)]
        void UpdatePlayerStatus(string code, string gamertag, bool isReady);

        /// <summary>
        /// Permite a un jugador abandonar un lobby.
        /// </summary>
        /// <param name="code">Código único del lobby.</param>
        /// <param name="gamertag">Gamertag del jugador que abandona el lobby.</param>
        [OperationContract]
        void LeaveLobby(string code, string gamertag);

        /// <summary>
        /// Inicia el juego dentro de un lobby.
        /// </summary>
        /// <param name="lobbyCode">Código único del lobby donde se inicia el juego.</param>
        [OperationContract(IsOneWay = true)]
        void PlayGame(string lobbyCode);
    }

    /// <summary>
    /// Define las operaciones de callback que el servidor puede invocar en los clientes conectados.
    /// </summary>
    public interface ILobbyConnectionCallback
    {
        /// <summary>
        /// Recibe un mensaje enviado por otro jugador en el lobby.
        /// </summary>
        /// <param name="gamertag">Gamertag del jugador que envió el mensaje.</param>
        /// <param name="message">Contenido del mensaje recibido.</param>
        [OperationContract(IsOneWay = true)]
        void ReceiveMessage(string gamertag, string message);

        /// <summary>
        /// Actualiza la interfaz de los jugadores en el lobby con el estado actual de preparación.
        /// </summary>
        /// <param name="playerStatus">Diccionario donde las claves son los gamertags de los jugadores y los valores son sus estados de preparación.</param>
        [OperationContract(IsOneWay = true)]
        void Repaint(Dictionary<string, bool> playerStatus);

        /// <summary>
        /// Notifica a los clientes conectados que el juego ha comenzado.
        /// </summary>
        /// <param name="lobbyPlayers">Diccionario con los jugadores del lobby y sus estados de conexión.</param>
        [OperationContract(IsOneWay = true)]
        void StartGame(Dictionary<string, bool> lobbyPlayers);
    }
}
