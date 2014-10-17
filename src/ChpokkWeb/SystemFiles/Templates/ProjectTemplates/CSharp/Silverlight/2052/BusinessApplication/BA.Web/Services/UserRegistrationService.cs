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

    // TODO: 在部署应用程序时切换为安全终结点。
    //       只应使用 https 传递用户名和密码。
    //       为此，请将 EnableClientAccessAttribute 的 RequiresSecureEndpoint 属性设置为 true。
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       有关对域服务使用 https 的更多信息可在 MSDN 上找到。

    /// <summary>
    /// 负责注册用户的域服务。
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// 默认情况下要将用户添加到的角色。
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// 注意: 这是可用于启动应用程序的示例代码。
        //// 在成品代码中，您应提供 CAPTCHA 控制功能或验证用户的电子邮件地址，从而针对拒绝服务攻击提供缓解措施。

        /// <summary>
        /// 使用提供的 <see cref="RegistrationData"/> 和 <paramref name="password"/> 添加新用户。
        /// </summary>
        /// <param name="user">此用户的注册信息。</param>
        /// <param name="password">新用户的密码。</param>
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

            // 请在创建用户之前运行，以确保角色启用且默认角色可用。
            //
            // 如果角色管理器存在问题，则现在失败要好于在创建用户之后失败。
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // 注意: ASP.NET 默认情况下使用 SQL Server Express 创建用户数据库。
            // 如果您未安装 SQL Server Express，则 CreateUser 会失败。
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // 将该用户分配给默认角色。
            // 如果禁用角色管理，则此操作将失败。
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // 设置友好名称(配置文件设置)。
            // 如果 web.config 配置不正确，则此操作将失败。
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
    /// 可以从 <see cref="UserRegistrationService.CreateUser"/> 返回的值的枚举
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