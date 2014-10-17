namespace $safeprojectname$
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.Web.Profile;
    using System.Web.Security;
    using $safeprojectname$.Resources;

    // TODO: cambie a un extremo seguro al implementar la aplicación.
    //       El nombre de usuario y la contraseña solo se deben pasar mediante https.
    //       Para ello, establezca la propiedad RequiresSecureEndpoint de EnableClientAccessAttribute en true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       En MSDN encontrará más información sobre el uso de https con un servicio de dominio.

    /// <summary>
    /// Servicio de dominio responsable del registro de los usuarios.
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// Rol al que se agregará a los usuarios de forma predeterminada.
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// NOTA: éste es un código de ejemplo para poner en marcha la aplicación.
        //// En el código de producción debería proporcionar una estrategia de reducción frente a un ataque de denegación de servicio al proporcionar funcionalidad de control CAPTCHA o comprobar la dirección de correo electrónico del usuario.

        /// <summary>
        /// Agrega un nuevo usuario con los <see cref="RegistrationData"/> y la <paramref name="password"/> proporcionados.
        /// </summary>
        /// <param name="user">Información de registro de este usuario.</param>
        /// <param name="password">Contraseña del nuevo usuario.</param>
        [Invoke(HasSideEffects = true)]
        public CreateUserStatus CreateUser(RegistrationData user,
            [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            [RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName = "ValidationErrorBadPasswordStrength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            [StringLength(50, MinimumLength = 7, ErrorMessageResourceName = "ValidationErrorBadPasswordLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
            string password)
        {
            if (user == null)
            {
                throw new ArgumentNullException("user");
            }

            // Ejecute esto ANTES de crear el usuario para asegurarse de que los roles están habilitados y el rol predeterminado está disponible.
            //
            // Si hay un problema con el administrador de roles, es mejor que se produzca un error ahora y no una vez creado el usuario.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // NOTA: ASP.NET utiliza SQL Server Express de forma predeterminada para crear la base de datos de usuarios. 
            // Si no tiene instalado SQL Server Express, se producirá un error de CreateUser.
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // Asigne el usuario al rol predeterminado.
            // Se producirá un error si la administración de roles está deshabilitada.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // Establezca el nombre descriptivo (establecimiento de perfil).
            // Se producirá un error si web.config está configurado incorrectamente.
            ProfileBase profile = ProfileBase.Create(user.UserName, true);
            profile.SetPropertyValue("FriendlyName", user.FriendlyName);
            profile.Save();

            return CreateUserStatus.Success;
        }

        private static CreateUserStatus ConvertStatus(MembershipCreateStatus createStatus)
        {
            switch (createStatus)
            {
                case MembershipCreateStatus.Success: return CreateUserStatus.Success;
                case MembershipCreateStatus.InvalidUserName: return CreateUserStatus.InvalidUserName;
                case MembershipCreateStatus.InvalidPassword: return CreateUserStatus.InvalidPassword;
                case MembershipCreateStatus.InvalidQuestion: return CreateUserStatus.InvalidQuestion;
                case MembershipCreateStatus.InvalidAnswer: return CreateUserStatus.InvalidAnswer;
                case MembershipCreateStatus.InvalidEmail: return CreateUserStatus.InvalidEmail;
                case MembershipCreateStatus.DuplicateUserName: return CreateUserStatus.DuplicateUserName;
                case MembershipCreateStatus.DuplicateEmail: return CreateUserStatus.DuplicateEmail;
                default: return CreateUserStatus.Failure;
            }
        }
    }

    /// <summary>
    /// Enumeración de los valores que se pueden devolver desde <see cref="UserRegistrationService.CreateUser"/>
    /// </summary>
    public enum CreateUserStatus
    {
        Success = 0,
        InvalidUserName = 1,
        InvalidPassword = 2,
        InvalidQuestion = 3,
        InvalidAnswer = 4,
        InvalidEmail = 5,
        DuplicateUserName = 6,
        DuplicateEmail = 7,
        Failure = 8,
    }
}