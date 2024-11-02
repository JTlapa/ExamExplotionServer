using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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
        List<AccessoryManagement> GetPurchasedAccessories(int playerId);
    }

    [DataContract]
    public class AccessoryManagement
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
    }
}
