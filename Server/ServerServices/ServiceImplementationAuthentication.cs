using DataAccess.EntitiesManager;
using DataAccess;
using ServerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace ServerService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public partial class ServiceImplementation : IAuthenticationManager
    {
        public bool AddAccount(AccountManagement account)
        {
            bool dataEntered = false;
            
            Account accountToAdd = new Account();
            accountToAdd.name = account.Name + account.Lastname;
            accountToAdd.email = account.Email;
            accountToAdd.password = account.Password;
            accountToAdd.gamertag = account.Gamertag;

            Users user = new Users();

            var accountId = AccountManagerDB.AddAcount(accountToAdd);
            var userId = UserManagerDB.AddUser(user);

            if(accountId != -1 && userId != -1)
            {
                Player player = new Player();
                player.accountId = accountId;
                player.userId = userId;
                player.score = 0;
                player.wins = 0;
                PlayerManagerDB.RegisterPlayer(player);
                AddDefaultAccessory(userId);
                dataEntered = true;
            }
            return dataEntered;
           
        }

        private void AddDefaultAccessory(int userId)
        {
            PurchasedAccessory purchasedAccessory = new PurchasedAccessory();
            purchasedAccessory.accessoryId = 1;
            purchasedAccessory.playerId = userId;
            purchasedAccessory.inUse = true;
            PurchasedAccessoryManagerDB.AddPurchasedAccessory(purchasedAccessory);
        }
        public int Login(AccountManagement account)
        {
            Account accountToValidate = new Account();
            accountToValidate.gamertag = account.Gamertag;
            accountToValidate.password = account.Password;

            var accountId = AccountManagerDB.ValidateAccount(accountToValidate);
            return accountId;
        }

        public bool UpdatePassword(AccountManagement account)
        {
            Account accountToUpdate = new Account();
            accountToUpdate.gamertag = account.Gamertag;
            accountToUpdate.password = account.Password;
            return AccountManagerDB.UpdatePassword(accountToUpdate);
        }

        public bool VerifyExistingEmail(string email)
        {
            return AccountManagerDB.VerifyExistingEmail(email);
        }

        public bool VerifyExistingGamertag(string gamertag)
        {
            return AccountManagerDB.VerifyExistingGamertag(gamertag);
        }

        public int GetAccountIdByGamertag(string gamertag)
        {
            return AccountManagerDB.GetAccountIdByGamertag(gamertag);
        }

        public bool DeactivateAccount(string gamertag)
        {
            return AccountManagerDB.DeactivateAccount(gamertag);
        }
    }
}
