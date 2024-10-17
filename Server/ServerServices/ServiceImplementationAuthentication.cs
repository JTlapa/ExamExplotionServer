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
        int userId;
        public int GetUserIdFromCurrentSession()
        {
            return userId;
        }

        public bool Login(AccountM account)
        {
            Account accountToValidate = new Account();
            accountToValidate.gamertag = account.Gamertag;
            accountToValidate.password = account.Password;

            this.userId = AccountManagerDB.ValidateAccount(accountToValidate);

            if (this.userId != -1)
            {
                this.gamertag = account.Gamertag;
                return true;
            }

            return false;
        }
    }
}
