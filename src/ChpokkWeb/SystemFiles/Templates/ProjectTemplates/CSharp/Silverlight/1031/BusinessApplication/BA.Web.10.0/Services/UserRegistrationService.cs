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

    // ' TODO: Wechseln Sie beim Bereitstellen der Anwendung auf einen sicheren Endpunkt.
    //       Der Name und das Kennwort des Benutzers sollten ausschließlich mittels https übermittelt werden.
    //       Legen Sie dazu die Eigenschaft "RequiresSecureEndpoint" unter EnableClientAccessAttribute auf "true" fest.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Weitere Informationen zur Verwendung von https mit einem Domänendienst finden Sie auf MSDN.

    /// <summary>
    /// Domänendienst für das Registrieren von Benutzern.
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// Rolle, der Benutzer standardmäßig hinzugefügt werden.
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// HINWEIS: Es handelt sich hierbei um Beispielcode, damit Sie Ihr Anwendung starten können.
        //// Im Code für die Produktion sollte eine Abwehr gegen einen Denial-of-Service-Angriff enthalten sein, z. B. durch Bereitstellung einer CAPTCHA-Funktionalität oder die Verifizierung der E-Mail-Adresse des Benutzers.

        /// <summary>
        /// Fügt einen neuen Benutzer mit den angegebenen <see cref="RegistrationData"/> und <paramref name="password"/> hinzu.
        /// </summary>
        /// <param name="user">Die Registrierungsinformationen für diesen Benutzer.</param>
        /// <param name="password">Das Kennwort für den neuen Benutzer.</param>
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

            // Dieser Vorgang sollte ausgeführt werden, BEVOR der Benutzer erstellt wird, um sicherzustellen, dass Rollen aktiviert sind und die Standardrolle verfügbar ist.
            //
            // Falls es ein Problem mit dem Rollen-Manager gibt, ist es besser, wenn der Prozess vor dem Erstellen des Benutzers fehlschlägt als danach.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // HINWEIS: ASP.NET nutzt standardmäßig SQL Server Express zum Erstellen der Benutzerdatenbank. 
            // CreateUser schlägt fehl, wenn SQL Server Express nicht installiert ist.
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // Weisen Sie den Benutzer der Standardrolle zu.
            // Wenn die Rollenverwaltung deaktiviert ist, schlägt dieser Vorgang fehl.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // Weisen Sie den Anmeldenamen zu (Profileinstellung)
            // Dies schlägt fehl, wenn "web.config" falsch konfiguriert ist.
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
    /// Eine Enumeration der Werte, die von <see cref="UserRegistrationService.CreateUser"/> zurückgegeben werden können
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