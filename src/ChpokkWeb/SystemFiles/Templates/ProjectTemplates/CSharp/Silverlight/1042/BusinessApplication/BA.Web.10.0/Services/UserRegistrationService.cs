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

    // TODO: 응용 프로그램을 배포할 때 보안 끝점으로 전환합니다.
    //       https를 통해서만 사용자의 이름과 암호를 전달해야 합니다.
    //       이렇게 하려면 EnableClientAccessAttribute의 RequiresSecureEndpoint 속성을 true로 설정합니다.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       도메인 서비스에서 https를 사용하는 방법에 대한 자세한 내용은 MSDN에서 확인할 수 있습니다.

    /// <summary>
    /// 도메인 서비스는 사용자 등록을 담당합니다.
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// 사용자가 기본적으로 추가되는 역할입니다.
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// 참고: 응용 프로그램을 시작하는 샘플 코드입니다.
        //// 프로덕션 코드에서 CAPTCHA 컨트롤 기능을 제공하거나 사용자의 전자 메일 주소를 확인하여 서비스 거부 공격을 완화해야 합니다.

        /// <summary>
        /// 제공된 <see cref="RegistrationData"/> 및 <paramref name="password"/>를 가진 새 사용자를 추가합니다.
        /// </summary>
        /// <param name="user">이 사용자에 대한 등록 정보입니다.</param>
        /// <param name="password">새 사용자의 암호입니다.</param>
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

            // 사용자를 만들기 전에 실행하여 역할을 설정하고 기본 역할을 사용할 수 있는지 확인합니다.
            //
            // 역할 관리자에 문제가 있는 경우 사용자를 만든 이후에 실패하는 것보다 지금 실패하는 것이 낫습니다.
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // 참고: 기본적으로 ASP.NET에서는 SQL Server Express를 사용하여 사용자 데이터베이스를 만듭니다. 
            // SQL Server Express를 설치하지 않은 경우 CreateUser가 실패합니다.
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // 사용자를 기본 역할에 할당합니다.
            // 역할 관리를 사용하지 않도록 설정하면 실패합니다.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // 표시 이름(프로필 설정)을 설정합니다.
            // web.config가 잘못 구성된 경우 실패합니다.
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
    /// <see cref="UserRegistrationService.CreateUser"/>에서 반환될 수 있는 값 열거형입니다.
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