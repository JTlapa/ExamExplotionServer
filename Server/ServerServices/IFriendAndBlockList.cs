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
    /// Define las operaciones relacionadas con la gestión de amigos y lista de bloqueados de los jugadores.
    /// </summary>
    [ServiceContract]
    public interface IFriendAndBlockList
    {
        /// <summary>
        /// Agrega un jugador a la lista de amigos.
        /// </summary>
        /// <param name="friend">Objeto que contiene la información de los jugadores que serán amigos.</param>
        /// <returns>El ID del nuevo amigo agregado.</returns>
        [OperationContract]
        int AddFriend(FriendManagement friend);

        /// <summary>
        /// Elimina un jugador de la lista de amigos.
        /// </summary>
        /// <param name="friend">Objeto que contiene la información del amigo a eliminar.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario False.</returns>
        [OperationContract]
        bool RemoveFriend(FriendManagement friend);

        /// <summary>
        /// Obtiene la lista de gamertags de los amigos de un jugador.
        /// </summary>
        /// <param name="playerId">El ID del jugador cuyo listado de amigos se desea obtener.</param>
        /// <returns>Un diccionario con los IDs de los amigos y sus gamertags correspondientes.</returns>
        [OperationContract]
        Dictionary<int, string> GetFriendsGamertags(int playerId);

        /// <summary>
        /// Agrega un jugador a la lista de bloqueados.
        /// </summary>
        /// <param name="blockList">Objeto que contiene la información de los jugadores que serán bloqueados.</param>
        /// <returns>El ID del nuevo jugador bloqueado.</returns>
        [OperationContract]
        int AddBlock(BlockListManagement blockList);

        /// <summary>
        /// Elimina un jugador de la lista de bloqueados.
        /// </summary>
        /// <param name="blockList">Objeto que contiene la información del jugador a desbloquear.</param>
        /// <returns>True si la operación fue exitosa, de lo contrario False.</returns>
        [OperationContract]
        bool RemoveBlock(BlockListManagement blockList);

        /// <summary>
        /// Obtiene la lista de gamertags de los jugadores bloqueados por un jugador.
        /// </summary>
        /// <param name="playerId">El ID del jugador cuyo listado de bloqueados se desea obtener.</param>
        /// <returns>Un diccionario con los IDs de los jugadores bloqueados y sus gamertags correspondientes.</returns>
        [OperationContract]
        Dictionary<int, string> GetBlockedGamertags(int playerId);
    }

    /// <summary>
    /// Representa la gestión de los amigos entre dos jugadores.
    /// </summary>
    [DataContract]
    public class FriendManagement
    {
        private int player1Id;
        private int player2Id;

        /// <summary>
        /// Obtiene o establece el ID del primer jugador.
        /// </summary>
        [DataMember]
        public int Player1Id
        {
            get { return player1Id; }
            set { player1Id = value; }
        }

        /// <summary>
        /// Obtiene o establece el ID del segundo jugador.
        /// </summary>
        [DataMember]
        public int Player2Id
        {
            get { return player2Id; }
            set { player2Id = value; }
        }
    }

    /// <summary>
    /// Representa la gestión de los jugadores bloqueados por un jugador.
    /// </summary>
    [DataContract]
    public class BlockListManagement
    {
        private int playerId;
        private int blockedPlayerId;

        /// <summary>
        /// Obtiene o establece el ID del jugador que está bloqueando a otro.
        /// </summary>
        [DataMember]
        public int PlayerId
        {
            get { return playerId; }
            set { playerId = value; }
        }

        /// <summary>
        /// Obtiene o establece el ID del jugador bloqueado.
        /// </summary>
        [DataMember]
        public int BlockedPlayerId
        {
            get { return blockedPlayerId; }
            set { blockedPlayerId = value; }
        }
    }
}
