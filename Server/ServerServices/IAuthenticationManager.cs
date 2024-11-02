using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServerService
{
    [ServiceContract(SessionMode = SessionMode.Required)]
    interface IAuthenticationManager
    {
        [OperationContract]
        bool Login(AccountManagement account);

        [OperationContract]
        int GetAccountIdFromCurrentSession();

        [OperationContract]
        bool AddAccount(AccountManagement account);

        [OperationContract]
        bool VerifyExistingGamertag(string gamertag);

        [OperationContract]
        bool VerifyExistingEmail(string email);

        [OperationContract]
        bool UpdatePassword(AccountManagement account);
    }

    [DataContract]
    public class AccountManagement
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
