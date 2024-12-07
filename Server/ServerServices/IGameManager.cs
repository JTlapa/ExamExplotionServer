﻿using DataAccess;
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
    /// Define las operaciones para la gestión de partidas en un juego multijugador.
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IGameConnectionCallback))]
    public interface IGameManager
    {
        /// <summary>
        /// Conecta a un jugador a una partida.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <param name="gamertag">Gamertag del jugador que se conecta.</param>
        /// <returns>Verdadero si la conexión fue exitosa; falso en caso contrario.</returns>
        [OperationContract]
        bool ConnectGame(string gameCode, string gamertag);

        /// <summary>
        /// Inicializa el mazo de cartas para una partida.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <param name="playerCount">Número de jugadores en la partida.</param>
        [OperationContract(IsOneWay = true)]
        void InitializeDeck(string gameCode, int playerCount, string gamertag);

        /// <summary>
        /// Finaliza la partida y registra al jugador ganador.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <param name="winnerPlayerId">ID del jugador ganador.</param>
        /// <returns>Verdadero si la operación fue exitosa; falso en caso contrario.</returns>
        [OperationContract]
        bool EndGame(string gameCode, int winnerPlayerId);

        /// <summary>
        /// Notifica al servidor que el turno de un jugador ha terminado.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <param name="currentGamertag">Gamertag del jugador cuyo turno finalizó.</param>
        [OperationContract(IsOneWay = true)]
        void NotifyEndTurn(string gameCode, string currentGamertag);

        /// <summary>
        /// Obtiene el gamertag del jugador que tiene el turno actual.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <returns>Gamertag del jugador en turno.</returns>
        [OperationContract]
        string GetCurrentTurn(string gameCode);

        /// <summary>
        /// Obtiene los detalles de una partida.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <returns>Objeto <see cref="GameManagement"/> con los detalles de la partida.</returns>
        [OperationContract]
        GameManagement GetGame(string gameCode);

        /// <summary>
        /// Inicializa los turnos de los jugadores en una partida.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <param name="gamertags">Lista de gamertags de los jugadores.</param>
        [OperationContract(IsOneWay = true)]
        void InitializeGameTurns(string gameCode, List<string> gamertags);

        /// <summary>
        /// Notifica al cliente que su turno ha comenzado.
        /// </summary>
        /// <param name="gameCode">Código único de la partida.</param>
        /// <param name="nextGametag">Gamertag del jugador que tiene el siguiente turno.</param>
        [OperationContract(IsOneWay = true)]
        void NotifyClientOfTurn(string gameCode, string nextGametag);

        [OperationContract (IsOneWay = true)]
        void NotifyDrawCard(string gameCode, string gamertag, bool isTopCard);
        [OperationContract (IsOneWay = true)]
        void NotifyCardOnBoard(string gameCode, string path);
        [OperationContract(IsOneWay = true)]
        void SendShuffleDeck(string gameCode, Stack<CardManagement> gameDeck);
        [OperationContract(IsOneWay = true)]
        void RequestCard(string gameCode, string playerRequested, string playerRequesting);
        [OperationContract(IsOneWay = true)]
        void SendCardToPlayer(string gameCode, string playerRequesting, CardManagement cardToSend);
     }

    /// <summary>
    /// Define las operaciones de callback que el servidor puede invocar en los clientes conectados.
    /// </summary>
    public interface IGameConnectionCallback
    {
        /// <summary>
        /// Actualiza al cliente con el turno actual.
        /// </summary>
        /// <param name="gamertag">Gamertag del jugador en turno.</param>
        [OperationContract(IsOneWay = true)]
        void UpdateCurrentTurn(string gamertag);

        /// <summary>
        /// Sincroniza el temporizador del turno entre el servidor y el cliente.
        /// </summary>
        [OperationContract(IsOneWay = true)]
        void SyncTimer();
        [OperationContract(IsOneWay = true)]
        void RecivePlayerAndGameDeck(Stack<CardManagement> gameDeck, List<CardManagement> playerDeck);
        [OperationContract(IsOneWay = true)]
        void RemoveCardFromStack(bool isTopCard);
        [OperationContract(IsOneWay = true)]
        void PrintCardOnBoard(string path);
        [OperationContract(IsOneWay = true)]
        void ReceiveGameDeck(Stack<CardManagement> gameDeck);
        [OperationContract(IsOneWay = true)]
        void NotifyCardRequested(string gameCode, string playerRequesting);
        [OperationContract(IsOneWay = true)]
        void NotifyCardReceived(CardManagement card);
    }

    /// <summary>
    /// Representa una carta dentro del juego.
    /// </summary>
    [DataContract]
    public class CardManagement
    {
        private string cardName;
        private string cardPath;

        /// <summary>
        /// Nombre de la carta.
        /// </summary>
        [DataMember]
        public string CardName { get { return cardName; } set { cardName = value; } }

        /// <summary>
        /// Ruta de la imagen asociada a la carta.
        /// </summary>
        [DataMember]
        public string CardPath { get { return cardPath; } set { cardPath = value; } }
    }

    /// <summary>
    /// Representa los detalles de una partida.
    /// </summary>
    [DataContract]
    public class GameManagement
    {
        private int gameId;
        private string invitationCode;
        private string gameStatus;
        private int numberPlayers;
        private int timePerTurn;
        private int lives;
        private int hostPlayerId;
        private int winnerPlayerId;

        /// <summary>
        /// Identificador único de la partida.
        /// </summary>
        [DataMember]
        public int GameId { get { return gameId; } set { gameId = value; } }

        /// <summary>
        /// Código de invitación para unirse a la partida.
        /// </summary>
        [DataMember]
        public string InvitationCode { get { return invitationCode; } set { invitationCode = value; } }

        /// <summary>
        /// Estado actual de la partida.
        /// </summary>
        [DataMember]
        public string GameStatus { get { return gameStatus; } set { gameStatus = value; } }

        /// <summary>
        /// Número de jugadores en la partida.
        /// </summary>
        [DataMember]
        public int NumberPlayers { get { return numberPlayers; } set { numberPlayers = value; } }

        /// <summary>
        /// Tiempo asignado por turno en segundos.
        /// </summary>
        [DataMember]
        public int TimePerTurn { get { return timePerTurn; } set { timePerTurn = value; } }

        /// <summary>
        /// Número de vidas asignadas a los jugadores.
        /// </summary>
        [DataMember]
        public int Lives { get { return lives; } set { lives = value; } }

        /// <summary>
        /// ID del jugador anfitrión.
        /// </summary>
        [DataMember]
        public int HostPlayerId { get { return hostPlayerId; } set { hostPlayerId = value; } }

        /// <summary>
        /// ID del jugador ganador.
        /// </summary>
        [DataMember]
        public int WinnerPlayerId { get { return winnerPlayerId; } set { winnerPlayerId = value; } }
    }
}
