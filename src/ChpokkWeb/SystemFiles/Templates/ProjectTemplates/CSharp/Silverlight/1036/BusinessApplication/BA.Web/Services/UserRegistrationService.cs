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

    // TODO: basculez vers un point de terminaison sécurisé lors du déploiement de l'application.
    //       Le nom et le mot de passe de l'utilisateur doivent être passés uniquement à l'aide de https.
    //       Pour ce faire, affectez à la propriété RequiresSecureEndpoint sur EnableClientAccessAttribute la valeur true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Pour plus d'informations sur l'utilisation de https avec un service de domaine, consultez MSDN.

    /// <summary>
    /// Service de domaine chargé de l'inscription des utilisateurs.
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// Rôle auquel les utilisateurs seront ajoutés par défaut.
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// REMARQUE : Ceci est un exemple de code permettant de faire démarrer votre application.
        //// Dans le code de production, vous devez fournir une atténuation d'une attaque par déni de service en fournissant la fonctionnalité de contrôle CAPTCHA ou en vérifiant l'adresse de messagerie de l'utilisateur.

        /// <summary>
        /// Ajoute un nouvel utilisateur avec les <see cref="RegistrationData"/> et <paramref name="password"/> fournis.
        /// </summary>
        /// <param name="user">Informations d'inscription de cet utilisateur.</param>
        /// <param name="password">Mot de passe du nouvel utilisateur.</param>
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

            // Effectuer cette exécution AVANT de créer l'utilisateur pour vérifier que les rôles sont activés et que le rôle par défaut est disponible.
            //
            // Si le gestionnaire de rôles présente un problème, il est préférable que l'échec se produise maintenant plutôt qu'après la création de l'utilisateur.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // REMARQUE : ASP.NET utilise par défaut SQL Server Express pour créer la base de données utilisateur. 
            // CreateUser échoue si SQL Server Express n'est pas installé.
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // Assigner l'utilisateur au rôle par défaut.
            // Cette opération échoue si la gestion des rôles est désactivée.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // Définir le nom convivial (paramètre de profil).
            // Cette opération échoue si le fichier web.config n'est pas configuré correctement.
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
    /// Énumération des valeurs qui peuvent être retournées par <see cref="UserRegistrationService.CreateUser"/>
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