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

    // TODO: passare a un endpoint sicuro quando si distribuisce l'applicazione.
    //       La password e il nome dell'utente devono essere passati solo con https.
    //       A tale scopo, impostare la proprietà RequiresSecureEndpoint in EnableClientAccessAttribute su true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Ulteriori informazioni sull'utilizzo di https con un servizio del dominio sono disponibili su MSDN.

    /// <summary>
    /// Servizio del dominio responsabile della registrazione degli utenti.
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// Ruolo a cui verranno aggiunti gli utenti per impostazione predefinita.
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// NOTA: si tratta di un codice di esempio per l'avvio dell'applicazione.
        //// Nel codice di produzione è necessario prevenire un attacco di tipo Denial of Service fornendo la funzionalità di controllo CAPTCHA o verificando l'indirizzo di posta elettronica dell'utente.

        /// <summary>
        /// Aggiunge un nuovo utente con gli oggetti <see cref="RegistrationData"/> e <paramref name="password"/> forniti.
        /// </summary>
        /// <param name="user">Informazioni di registrazione dell'utente.</param>
        /// <param name="password">Password del nuovo utente.</param>
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

            // Eseguire questo metodo prima di creare l'utente per verificare che i ruoli siano abilitati e che il ruolo predefinito sia disponibile.
            //
            // Se si verifica un problema con la gestione ruoli, è preferibile che l'operazione venga interrotta ora anziché dopo la creazione dell'utente.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // NOTA: per impostazione predefinita, con ASP.NET viene utilizzato SQL Server Express per creare il database utente. 
            // Impossibile eseguire CreateUser se non è installato SQL Server Express.
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // Assegnare l'utente al ruolo predefinito.
            // Può verificarsi un errore se la gestione ruoli è disabilitata.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // Impostare il relativo nome descrittivo (impostazione profilo).
            // Può verificarsi un errore se web.config non è configurato correttamente.
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
    /// Enumerazione dei valori che possono essere restituiti da <see cref="UserRegistrationService.CreateUser"/>
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