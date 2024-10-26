using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DataAccess;
using DataAccess.EntitiesManager;

namespace XUnitTest
{
    public class AccountManagerDBTest
    {
        [Fact]
        public void AddAccountSuccessTest()
        {
            var account = new Account
            {
                email = "elrevo@uv.mx",
                password = "password",
                gamertag = "elrevo"
            };

            int result = AccountManagerDB.AddAcount(account);

            Assert.NotEqual(-1, result);
        }

        [Fact]
        public void AddAccountFailTest()
        {
            Account nullAccount = null;

            var result = AccountManagerDB.AddAcount(nullAccount);

            Assert.Equal(-1, result);
        }

        [Fact]
        public void ValidateAccountSuccessTest()
        {
            var account = new Account
            {
                email = "tlapa11@hotmail.com",
                password = "password",
                gamertag = "tlapa38"
            };

            AccountManagerDB.AddAcount(account);

            int result = AccountManagerDB.ValidateAccount(account);
            Assert.NotEqual(-1, result);
        }

        [Fact]
        public void UpdatePasswordSuccessTest()
        {
            var account = new Account
            {
                email = "zaidskate@hotmail.com",
                password = "password",
                gamertag = "zaid0"
            };

            AccountManagerDB.AddAcount(account);

            bool result = AccountManagerDB.UpdatePassword(account);
            Assert.True(result);
        }

        [Fact]
        public void UpdatePasswordFailTest()
        {
            var account = new Account
            {
                email = "zs22013693@estudiantes.uv.mx",
                password = "password",
                gamertag = "zaids"
            };

            bool result = AccountManagerDB.UpdatePassword(account);
            Assert.False(result);
        }

        [Fact]
        public void VerifyExistingGamertagSuccessTest()
        {
            var account = new Account
            {
                email = "zaidskate@hotmail.com",
                password = "password",
                gamertag = "zaid0"
            };

            AccountManagerDB.AddAcount(account);

            bool result = AccountManagerDB.VerifyExistingGamertag("zaid0");
            Assert.True(result);
        }

        [Fact]
        public void VerifyExistingGamertagFailTest()
        {
            bool result = AccountManagerDB.VerifyExistingGamertag("gamertagInexistente");
            Assert.False(result);
        }

        [Fact]
        public void VerifyExistingEmailSuccessTest()
        {
            var account = new Account
            {
                email = "zaidoskate@hotmail.com",
                password = "password",
                gamertag = "zaid19"
            };
            AccountManagerDB.AddAcount(account);
            bool result = AccountManagerDB.VerifyExistingEmail("zaidoskate@hotmail.com");
            Assert.True(result);
        }

        [Fact]
        public void VerifyExistingEmailFailTest()
        {
            bool result = AccountManagerDB.VerifyExistingEmail("estoNoEsUnCorreo");
            Assert.False(result);
        }

    }
}
