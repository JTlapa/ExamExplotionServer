using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServerService
{
    
    [ServiceContract]
    interface IAccountManager
    {
        [OperationContract]
        bool validateLogIn(AccountM account);

    }

    [DataContract]
    public class AccountM
    {
        private String gamertag;
        private String password;

        [DataMember]
        public String Gamertag { get { return gamertag; } set { gamertag = value; } }

        [DataMember]
        public String Password { get { return password; } set { password = value; } }

    }
}
