using DataAccess.Helpers;
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
    public static class BlockListManagerDB
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(BlockListManagerDB));

        public static int AddBlock(BlockList blockList)
        {
            int idBlockAdded = -1;
            if (!IsPlayerBlocked(blockList))
            {
                try
                {
                    using (var context = new ExamExplotionDBEntities())
                    {
                        var newBlock = context.BlockList.Add(blockList);
                        context.SaveChanges();
                        if (newBlock != null)
                        {
                            idBlockAdded = newBlock.idBlock;
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
            }
            else
            {
                idBlockAdded = -2;
            }
            return idBlockAdded;
        }
        public static bool IsPlayerBlocked(BlockList blockList)
        {
            bool isBlocked = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    isBlocked = context.BlockList.Any(b =>
                        (b.idPlayer == blockList.idPlayer && b.blockedPlayer == blockList.blockedPlayer) ||
                        (b.blockedPlayer == blockList.idPlayer && b.idPlayer == blockList.blockedPlayer));
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
            return isBlocked;
        }
        public static Dictionary<int, string> GetGamertagsBlocked(int playerId)
        {
            Dictionary<int, string> blockedGamertags = new Dictionary<int, string>();
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    blockedGamertags = context.BlockList
                        .Where(b => b.idPlayer == playerId)
                        .Join(context.Player,
                              block => block.blockedPlayer,
                              player => player.userId,
                              (block, player) => new { player.userId, player.accountId })
                        .Join(context.Account,
                              playerInfo => playerInfo.accountId,
                              account => account.accountId,
                              (playerInfo, account) => new { playerInfo.userId, account.gamertag })
                        .ToDictionary(entry => entry.userId, entry => entry.gamertag);
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
            return blockedGamertags;
        }
        public static bool RemoveBlockList(BlockList blockList)
        {
            bool isRemoved = false;
            try
            {
                using (var context = new ExamExplotionDBEntities())
                {
                    var blockToRemove = context.BlockList
                        .FirstOrDefault(b =>
                            b.idPlayer == blockList.idPlayer &&
                            b.blockedPlayer == blockList.blockedPlayer);
                    if (blockToRemove != null)
                    {
                        context.BlockList.Remove(blockToRemove);
                        context.SaveChanges();
                        isRemoved = true;
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
            return isRemoved;
        }

    }
}
