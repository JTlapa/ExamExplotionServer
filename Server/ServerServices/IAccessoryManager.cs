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
    interface IAccessoryManager
    {
        [OperationContract]
        bool PurchaseAccessory(int playerId, int accessoryId);

        [OperationContract]
        bool SetAccessoryInUse(int playerId, int accessoryId);

        [OperationContract]
        List<AccessoryM> GetPurchasedAccessories(int playerId);
    }

    [DataContract]
    public class AccessoryM
    {
        private int accessoryId;
        private int price;
        private string accessoryName;
        private string description;
        private string imagesPackage;

        [DataMember]
        public int AccesoryId { get { return accessoryId; } set { accessoryId = value; } }
        [DataMember]
        public string AccessoryName { get { return accessoryName; } set { accessoryName = value; } }
        [DataMember]
        public int Price { get { return price; } set { price = value; } }
        [DataMember]
        public string Description { get { return description; } set { description = value; } }
        [DataMember]
        public string ImagesPackage { get { return imagesPackage; } set { imagesPackage = value; } }
=======
namespace ServerService
{
    /// <summary>
    /// Define las operaciones relacionadas con la gestión de accesorios en el sistema.
    /// </summary>
    [ServiceContract]
    interface IAccessoryManager
    {
        /// <summary>
        /// Realiza la compra de un accesorio para un jugador.
        /// </summary>
        /// <param name="purchasedAccessoryManagement">Objeto <see cref="PurchasedAccessoryManagement"/> con los detalles de la compra.</param>
        /// <returns>Verdadero si la compra fue exitosa; falso en caso contrario.</returns>
        [OperationContract]
        bool PurchaseAccessory(PurchasedAccessoryManagement purchasedAccessoryManagement);

        /// <summary>
        /// Establece un accesorio como "en uso" para un jugador.
        /// </summary>
        /// <param name="purchasedAccessoryManagement">Objeto <see cref="PurchasedAccessoryManagement"/> con los detalles del accesorio.</param>
        /// <returns>Verdadero si la operación fue exitosa; falso en caso contrario.</returns>
        [OperationContract]
        bool SetAccessoryInUse(PurchasedAccessoryManagement purchasedAccessoryManagement);

        /// <summary>
        /// Obtiene una lista de identificadores de accesorios comprados por un jugador.
        /// </summary>
        /// <param name="playerId">Identificador del jugador.</param>
        /// <returns>Lista de identificadores de accesorios comprados.</returns>
        [OperationContract]
        List<int> GetPurchasedAccessories(int playerId);

        /// <summary>
        /// Obtiene el accesorio actualmente en uso por un jugador.
        /// </summary>
        /// <param name="playerId">Identificador del jugador.</param>
        /// <returns>Objeto <see cref="AccessoryManagement"/> con los detalles del accesorio en uso.</returns>
        [OperationContract]
        AccessoryManagement GetAccessoryInUse(int playerId);
    }

    /// <summary>
    /// Representa los detalles de un accesorio en el sistema.
    /// </summary>
    [DataContract]
    public class AccessoryManagement
    {
        private int accessoryId;
        private string accessoryName;
        private string path;

        /// <summary>
        /// Identificador único del accesorio.
        /// </summary>
        [DataMember]
        public int AccessoryId { get { return accessoryId; } set { accessoryId = value; } }

        /// <summary>
        /// Nombre del accesorio.
        /// </summary>
        [DataMember]
        public string AccessoryName { get { return accessoryName; } set { accessoryName = value; } }

        /// <summary>
        /// Ruta del recurso asociado al accesorio.
        /// </summary>
        [DataMember]
        public string Path { get { return path; } set { path = value; } }
    }

    /// <summary>
    /// Representa los detalles de un accesorio comprado por un jugador.
    /// </summary>
    [DataContract]
    public class PurchasedAccessoryManagement
    {
        private int accessoryId;
        private int playerId;
        private bool inUse;

        /// <summary>
        /// Identificador único del accesorio.
        /// </summary>
        [DataMember]
        public int AccesoryId { get { return accessoryId; } set { accessoryId = value; } }

        /// <summary>
        /// Identificador del jugador que compró el accesorio.
        /// </summary>
        [DataMember]
        public int PlayerId { get { return playerId; } set { playerId = value; } }

        /// <summary>
        /// Indica si el accesorio está actualmente en uso.
        /// </summary>
        [DataMember]
        public bool InUse { get { return inUse; } set { inUse = value; } }
>>>>>>> f796509853e9a2b1f8b30832f75c8529577581bf
    }
}
