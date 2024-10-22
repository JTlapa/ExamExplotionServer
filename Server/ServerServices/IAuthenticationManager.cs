using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServerService
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    interface IAuthenticationManager
    {
        [OperationContract]
        bool Login(AccountM account);

        [OperationContract]
        int GetAccountIdFromCurrentSession();

        [OperationContract]
        bool AddAccount(AccountM account);

        [OperationContract]
        bool VerifyExistingGamertag(string gamertag);

        [OperationContract]
        bool VerifyExistingEmail(string email);

        [OperationContract]
        bool UpdatePassword(AccountM account);
    }

    [DataContract]
    public class AccountM
    {
        private String name;
        private String lastname;
        private String email;
        private String gamertag;
        private String password;

        [DataMember]
        public String Name { get { return name; } set { name = value; } }

        [DataMember]
        public String Lastname { get { return lastname; } set { lastname = value; } }

        [DataMember]
        public String Email { get { return email; } set { email = value; } }

        [DataMember]
        public String Gamertag { get { return gamertag; } set { gamertag = value; } }

        [DataMember]
        public String Password { get { return password; } set { password = value; } }

    }
}
