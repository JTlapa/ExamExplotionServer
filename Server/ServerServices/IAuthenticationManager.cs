using System;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace ServerService
{
    /// <summary>
    /// Define las operaciones relacionadas con la autenticación y gestión de cuentas de usuario.
    /// </summary>
    [ServiceContract(SessionMode = SessionMode.Required)]
    interface IAuthenticationManager
    {
        /// <summary>
        /// Autentica a un usuario utilizando sus credenciales.
        /// </summary>
        /// <param name="account">Objeto <see cref="AccountManagement"/> con las credenciales del usuario.</param>
        /// <returns>Verdadero si la autenticación es exitosa; falso en caso contrario.</returns>
        [OperationContract]
        bool Login(AccountManagement account);

        /// <summary>
        /// Registra una nueva cuenta en el sistema.
        /// </summary>
        /// <param name="account">Objeto <see cref="AccountManagement"/> con los detalles de la cuenta a registrar.</param>
        /// <returns>Verdadero si la cuenta fue registrada exitosamente; falso en caso contrario.</returns>
        [OperationContract]
        bool AddAccount(AccountManagement account);

        /// <summary>
        /// Verifica si un gamertag ya está registrado en el sistema.
        /// </summary>
        /// <param name="gamertag">Gamertag a verificar.</param>
        /// <returns>Verdadero si el gamertag ya existe; falso en caso contrario.</returns>
        [OperationContract]
        bool VerifyExistingGamertag(string gamertag);

        /// <summary>
        /// Verifica si un correo electrónico ya está registrado en el sistema.
        /// </summary>
        /// <param name="email">Correo electrónico a verificar.</param>
        /// <returns>Verdadero si el correo ya existe; falso en caso contrario.</returns>
        [OperationContract]
        bool VerifyExistingEmail(string email);

        /// <summary>
        /// Actualiza la contraseña de una cuenta.
        /// </summary>
        /// <param name="account">Objeto <see cref="AccountManagement"/> con los datos de la cuenta, incluyendo la nueva contraseña.</param>
        /// <returns>Verdadero si la contraseña fue actualizada exitosamente; falso en caso contrario.</returns>
        [OperationContract]
        bool UpdatePassword(AccountManagement account);

        /// <summary>
        /// Obtiene el ID de una cuenta a partir de su gamertag.
        /// </summary>
        /// <param name="gamertag">Gamertag del usuario.</param>
        /// <returns>ID de la cuenta asociada al gamertag.</returns>
        [OperationContract]
        int GetAccountIdByGamertag(string gamertag);
    }

    /// <summary>
    /// Representa los detalles de una cuenta de usuario.
    /// </summary>
    [DataContract]
    public class AccountManagement
    {
        private string name;
        private string lastname;
        private string email;
        private string gamertag;
        private string password;

        /// <summary>
        /// Nombre del usuario.
        /// </summary>
        [DataMember]
        public string Name { get { return name; } set { name = value; } }

        /// <summary>
        /// Apellido del usuario.
        /// </summary>
        [DataMember]
        public string Lastname { get { return lastname; } set { lastname = value; } }

        /// <summary>
        /// Correo electrónico del usuario.
        /// </summary>
        [DataMember]
        public string Email { get { return email; } set { email = value; } }

        /// <summary>
        /// Gamertag del usuario.
        /// </summary>
        [DataMember]
        public string Gamertag { get { return gamertag; } set { gamertag = value; } }

        /// <summary>
        /// Contraseña de la cuenta del usuario.
        /// </summary>
        [DataMember]
        public string Password { get { return password; } set { password = value; } }
    }
}
