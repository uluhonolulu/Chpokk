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

    // TODO: 部署此應用程式時切換到安全端點。
    //       使用者的名稱和密碼只可以使用 https 傳遞。
    //       若要執行此動作，請將 EnableClientAccessAttribute 上的 RequiresSecureEndpoint 屬性設定為 true。
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       可以在 MSDN 上取得搭配  https 使用網域服務的詳細資訊。

    /// <summary>
    /// 網域服務負責註冊使用者。
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// 使用者預設要加入的目標角色。
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// 注意: 這是讓應用程式啟動的範例程式碼。
        //// 在實際執行的程式碼中，您應該提供 CAPTCHA 控制功能或驗證使用者的電子郵件地址來緩和服務阻斷攻擊。

        /// <summary>
        /// 使用提供的 <see cref="RegistrationData"/> 和 <paramref name="password"/> 加入新的使用者。
        /// </summary>
        /// <param name="user">這位使用者的註冊資訊。</param>
        /// <param name="password">新使用者的密碼。</param>
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

            // 請於建立使用者之前執行，以確保角色會啟用，而且預設角色可供使用。
            //
            // 如果角色管理員有問題，最好立即產生失敗，不要等到使用者建立之後才產生失敗。
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // 注意: 根據預設，ASP.NET 會使用 SQL Server Express 來建立使用者資料庫。
            // 如果未安裝 SQL Server Express，CreateUser 將會失敗。
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // 將使用者指派到預設角色。
            // 如果停用角色管理，它將會失敗。
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // 設定易記名稱 (設定檔設定)。
            // 如果 web.config 設定不正確，它將會失敗。
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
    /// 列舉可以從 <see cref="UserRegistrationService.CreateUser"/> 傳回的值
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