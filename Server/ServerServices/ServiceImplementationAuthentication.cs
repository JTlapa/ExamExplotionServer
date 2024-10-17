using DataAccess.EntitiesManager;
using DataAccess;
using ServerService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace ServerService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]
    public partial class ServiceImplementation : IAuthenticationManager
    {
        string gamertag;
        int accountId;
        int userId;

        public bool AddAccount(AccountM account)
        {
            bool dataEntered = false;

            Account accountToAdd = new Account();
            accountToAdd.name = account.Name + account.Lastname;
            accountToAdd.email = account.Email;
            accountToAdd.password = account.Password;
            accountToAdd.gamertag = account.Gamertag;

            Users user = new Users();

            accountId = AccountManagerDB.AddAcount(accountToAdd);
            userId = UserManagerDB.AddUser(user);

            if(accountId != -1 && userId != -1)
            {
                Player player = new Player();
                player.accountId = accountId;
                player.userId = userId;
                player.score = 0;
                player.wins = 0;
                PlayerManagerDB.RegisterPlayer(player);
                dataEntered = true;
            }
            return dataEntered;
        }

        public int GetAccountIdFromCurrentSession()
        {
            return accountId;
        }

        public bool Login(AccountM account)
        {
            Account accountToValidate = new Account();
            accountToValidate.gamertag = account.Gamertag;
            accountToValidate.password = account.Password;

            this.accountId = AccountManagerDB.ValidateAccount(accountToValidate);
            if (this.accountId != -1)
            {
                this.gamertag = account.Gamertag;
                return true;
            }

            return false;
        }
    }
}
