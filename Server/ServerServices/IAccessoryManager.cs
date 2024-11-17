using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    [ServiceContract]
    interface IAccessoryManager
    {
        [OperationContract]
        bool PurchaseAccessory(PurchasedAccessoryManagement purchasedAccessoryManagement);

        [OperationContract]
        bool SetAccessoryInUse(PurchasedAccessoryManagement purchasedAccessoryManagement);

        [OperationContract]
        List<int> GetPurchasedAccessories(int playerId);

        [OperationContract]
        AccessoryManagement GetAccessoryInUse(int playerId);
    }


    [DataContract]
    public class AccessoryManagement
    {
        private int accessoryId;
        private string accessoryName;
        private string path;
        [DataMember]
        public int AccessoryId { get { return accessoryId; } set { accessoryId = value; } }
        [DataMember]
        public string AccessoryName { get { return accessoryName; } set { accessoryName = value; } }
        [DataMember]
        public string Path { get { return path; } set { path = value; } }
    }

    [DataContract]
    public class PurchasedAccessoryManagement
    {
        private int accessoryId;
        private int playerId;
        private bool inUse;

        [DataMember]
        public int AccesoryId { get { return accessoryId; } set { accessoryId = value; } }
        [DataMember]
        public int PlayerId { get { return playerId; } set { playerId = value; } }
        [DataMember]
        public bool InUse { get { return inUse; } set { inUse = value; } }
    }
}
