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

    // TODO: переключиться на защищенную конечную точку при развертывании приложения.
    //       Имя и пароль пользователя должны передаваться только по протоколу https.
    //       Для этого нужно установить свойство RequiresSecureEndpoint элемента EnableClientAccessAttribute в значение true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Дополнительные сведения об использовании протокола https в службе домена см. в MSDN.

    /// <summary>
    /// Служба домена отвечает за регистрацию пользователей.
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// Роль, в которую пользователи будут добавляться по умолчанию.
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// ПРИМЕЧАНИЕ. Это образец кода, с которого можно начать разработку своего приложения.
        //// В коде рабочей системы необходимо предусмотреть защиту от атак типа "отказ в обслуживании", добавив элемент управления CAPTCHA или проверку адреса электронной почты.

        /// <summary>
        /// Добавляет нового пользователя с указанными <see cref="RegistrationData"/> и <paramref name="password"/>.
        /// </summary>
        /// <param name="user">Регистрационные данные пользователя.</param>
        /// <param name="password">Пароль нового пользователя.</param>
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

            // Это нужно выполнить ДО создания пользователя, чтобы удостовериться, что нужные роли разрешены и доступна роль по умолчанию.
            //
            // Если возникнет проблема с диспетчером ролей, то лучше выдать ошибку сейчас, чем после создания пользователя.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // ПРИМЕЧАНИЕ. ASP.NET по умолчанию для создания базы данных пользователей использует SQL Server Express. 
            // Вызов метода CreateUser завершится ошибкой, если не установлен SQL Server Express.
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // Назначить пользователя ролью по умолчанию.
            // Вызов завершится ошибкой, если отключено управление ролями.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // Задает понятное имя (параметр профиля).
            // Вызов завершится ошибкой, если неверно настроен web.config.
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
    /// Перечисление, содержащее значения, которые могут быть возвращены методом <see cref="UserRegistrationService.CreateUser"/>
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