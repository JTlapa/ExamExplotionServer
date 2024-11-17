using DataAccess;
using DataAccess.EntitiesManager;
using ServerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    public partial class ServiceImplementation : IAccessoryManager
    {
        public List<int> GetPurchasedAccessories(int playerId)
        {
            List<int> purchasedAccessories = PurchasedAccessoryManagerDB.GetPurchasedAccessoriesIdByPlayer(playerId);
            return purchasedAccessories;
        }

        public bool PurchaseAccessory(PurchasedAccessoryManagement purchasedAccessoryManagement)
        {
            PurchasedAccessory purchasedAccessory = new PurchasedAccessory();
            purchasedAccessory.accessoryId = purchasedAccessoryManagement.AccesoryId;
            purchasedAccessory.playerId = purchasedAccessoryManagement.PlayerId;
            purchasedAccessory.inUse = purchasedAccessoryManagement.InUse;

            bool accessoryPurchased = PurchasedAccessoryManagerDB.AddPurchasedAccessory(purchasedAccessory);
            return accessoryPurchased;
        }

        public bool SetAccessoryInUse(PurchasedAccessoryManagement purchasedAccessoryManagement)
        {
            PurchasedAccessory purchasedAccessory = new PurchasedAccessory();
            purchasedAccessory.accessoryId = purchasedAccessoryManagement.AccesoryId;
            purchasedAccessory.playerId = purchasedAccessoryManagement.PlayerId;
            purchasedAccessory.inUse = purchasedAccessoryManagement.InUse;

            bool accessoryUsed = PurchasedAccessoryManagerDB.UpdatePurchasedAccessoryInUse(purchasedAccessory);
            return accessoryUsed;
        }

        public AccessoryManagement GetAccessoryInUse(int playerId)
        {
            var accessory = PurchasedAccessoryManagerDB.GetAccessoryInUse(playerId);
            AccessoryManagement accessoryManagement = new AccessoryManagement();
            accessoryManagement.AccessoryId = accessory.accessoryId;
            accessoryManagement.AccessoryName = accessory.name;
            accessoryManagement.Path = accessory.imagesPackage;
            return accessoryManagement;
        }
    }
}
