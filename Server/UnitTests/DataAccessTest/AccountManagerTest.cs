using DataAccess;
using DataAccess.EntitiesManager;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    [TestClass]
    public class AccountManagerTest
    {
        [TestMethod]
        public void TestValidateAccountSuccess()
        {
            Account accountToValidate = new Account();
            accountToValidate.gamertag = "JesusTlapa11";
            accountToValidate.password = "123456789";

            int currentResult = AccountManagerDB.ValidateAccount(accountToValidate);
            int expectedResult = 7;

            Assert.AreEqual(expectedResult, currentResult);
        }
    }
}
