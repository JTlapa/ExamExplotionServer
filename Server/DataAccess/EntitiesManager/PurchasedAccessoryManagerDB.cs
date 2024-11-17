using DataAccess.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public class PurchasedAccessoryManagerDB
    {
        public static bool AddPurchasedAccessory(PurchasedAccessory purchasedAccessory)
        {
            bool purchasedAccessoryAdded = false;
            ChargeAccessoryPrice(purchasedAccessory);
            using (var context = new ExamExplotionDBEntities())
            {
                var newPurchasedAccessory = context.PurchasedAccessory.Add(purchasedAccessory);
                context.SaveChanges();
                if (newPurchasedAccessory != null)
                {
                    purchasedAccessoryAdded = true;
                }
            }
            return purchasedAccessoryAdded;
        }

        private static void ChargeAccessoryPrice(PurchasedAccessory purchasedAccessory)
        {
            int amount = 0;
            using (var context = new ExamExplotionDBEntities())
            {
                var accessory = context.Accessory.FirstOrDefault(a => a.accessoryId == purchasedAccessory.accessoryId);
                if (accessory != null)
                {
                    amount = 0 - accessory.price;
                }
            }
            PlayerManagerDB.UpdateScore(purchasedAccessory.playerId, amount);
        }

        private static bool RemoveePurchasedAccessoryInUse(int playerId)
        {
            bool purchasedAccessoryUpdated = false;
            using (var context = new ExamExplotionDBEntities())
            {
                var purchasedAccessoryToUpdate = context.PurchasedAccessory.FirstOrDefault(p => p.playerId == playerId && p.inUse == true );
                if (purchasedAccessoryToUpdate != null)
                {
                    purchasedAccessoryToUpdate.inUse = false;
                    context.SaveChanges();
                    purchasedAccessoryUpdated = true;
                }
            }
            return purchasedAccessoryUpdated;
        }
        public static bool UpdatePurchasedAccessoryInUse(PurchasedAccessory purchasedAccessory)
        {
            bool purchasedAccessoryUpdated = false;

            bool purchasedAccessoryInUseRemoved = RemoveePurchasedAccessoryInUse(purchasedAccessory.playerId);
            if( purchasedAccessoryInUseRemoved)
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var purchasedAccessoryToUpdate = context.PurchasedAccessory.FirstOrDefault(p => p.playerId == purchasedAccessory.playerId && p.accessoryId == purchasedAccessory.accessoryId);
                    if (purchasedAccessoryToUpdate != null)
                    {
                        purchasedAccessoryToUpdate.inUse = true;
                        context.SaveChanges();
                        purchasedAccessoryUpdated = true;
                    }
                }
            }
            return purchasedAccessoryUpdated;
        }
        public static List<int> GetPurchasedAccessoriesIdByPlayer(int playerId)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var purchasedAccessoriesIds = context.PurchasedAccessory.Where(p => p.playerId == playerId).Select(p => p.accessoryId).ToList();

                return purchasedAccessoriesIds;
            }
        }
        public static Accessory GetAccessoryInUse(int playerId)
        {
            using (var context = new ExamExplotionDBEntities())
            {
                var purchasedAccessory = context.PurchasedAccessory.FirstOrDefault(p => p.playerId == playerId && p.inUse == true);
                var accessory = context.Accessory.FirstOrDefault(p => p.accessoryId ==  purchasedAccessory.accessoryId);
                if(accessory == null)
                {
                    accessory = new Accessory();
                    accessory.accessoryId = 1;
                    accessory.price = 0;
                }
                return accessory;
            }
        }
    }
}
