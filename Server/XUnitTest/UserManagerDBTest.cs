using DataAccess.EntitiesManager;
using DataAccess;
using Xunit;
using System.Configuration;

namespace XUnitTest
{
    public class UserManagerDBTest
    {
        [Fact]
        public void AddUserTestSuccess()
        {
            Users user = new Users();
            int result = UserManagerDB.AddUser(user);
            Assert.NotEqual(-1, result);
        }
    }
}