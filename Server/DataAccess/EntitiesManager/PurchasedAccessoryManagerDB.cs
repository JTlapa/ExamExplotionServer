﻿using DataAccess.Helpers;
using log4net;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.EntitiesManager
{
    public static class PurchasedAccessoryManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(PurchasedAccessoryManagerDB));
        public static bool AddPurchasedAccessory(PurchasedAccessory purchasedAccessory)
        {
            bool purchasedAccessoryAdded = false;
            ChargeAccessoryPrice(purchasedAccessory);
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var newPurchasedAccessory = context.PurchasedAccessory.Add(purchasedAccessory);
                    context.SaveChanges();
                    if (newPurchasedAccessory != null)
                    {
                        purchasedAccessoryAdded = true;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return purchasedAccessoryAdded;
        }

        private static void ChargeAccessoryPrice(PurchasedAccessory purchasedAccessory)
        {
            int amount = 0;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var accessory = context.Accessory.FirstOrDefault(a => a.accessoryId == purchasedAccessory.accessoryId);
                    if (accessory != null)
                    {
                        amount = 0 - accessory.price;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            PlayerManagerDB.UpdateScore(purchasedAccessory.playerId, amount);
        }

        private static bool RemoveePurchasedAccessoryInUse(int playerId)
        {
            bool purchasedAccessoryUpdated = false;
            try
            {
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
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return purchasedAccessoryUpdated;
        }
        public static bool UpdatePurchasedAccessoryInUse(PurchasedAccessory purchasedAccessory)
        {
            bool purchasedAccessoryUpdated = false;

            bool purchasedAccessoryInUseRemoved = RemoveePurchasedAccessoryInUse(purchasedAccessory.playerId);
            if( purchasedAccessoryInUseRemoved)
            {
                try
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
                catch (SqlException sqlException)
                {
                    log.Error(sqlException);
                }
                catch (InvalidOperationException invalidOperationException)
                {
                    log.Warn(invalidOperationException);
                }
                catch (EntityException entityException)
                {
                    log.Error(entityException);
                }
                catch (Exception exception)
                {
                    log.Error(exception);
                }
            }
            return purchasedAccessoryUpdated;
        }
        public static List<int> GetPurchasedAccessoriesIdByPlayer(int playerId)
        {
            List<int> purchasedAccessoriesIds = new List<int>();
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    purchasedAccessoriesIds = context.PurchasedAccessory.Where(p => p.playerId == playerId).Select(p => p.accessoryId).ToList();
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return purchasedAccessoriesIds;
        }
        public static Accessory GetAccessoryInUse(int playerId)
        {
            Accessory accessory = new Accessory();
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var purchasedAccessory = context.PurchasedAccessory.FirstOrDefault(p => p.playerId == playerId && p.inUse == true);
                    accessory = context.Accessory.FirstOrDefault(p => p.accessoryId ==  purchasedAccessory.accessoryId);
                    if(accessory == null)
                    {
                        accessory = new Accessory();
                        accessory.accessoryId = 1;
                        accessory.price = 0;
                    }
                }
            }
            catch (SqlException sqlException)
            {
                log.Error(sqlException);
            }
            catch (InvalidOperationException invalidOperationException)
            {
                log.Warn(invalidOperationException);
            }
            catch (EntityException entityException)
            {
                log.Error(entityException);
            }
            catch (Exception exception)
            {
                log.Error(exception);
            }
            return accessory;
        }
    }
}
