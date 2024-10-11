using DataAccess;
using DataAccess.EntitiesManager;

namespace ServerService
{
    public partial class ServiceImplementation : IAccountManager
    {
        public bool validateLogIn(AccountM account)
        {
            Account accountToValidate = new Account();
            accountToValidate.gamertag = account.Gamertag;
            accountToValidate.password = account.Password;

            bool result = AccountManagerDB.ValidateAccount(accountToValidate);

            return result;
        }
    }
}
