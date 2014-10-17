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

    // TODO: アプリケーションを配置するときに安全なエンドポイントに切り替えます。
    //       ユーザーの名前とパスワードは、https のみを使用して渡す必要があります。
    //       そのためには、EnableClientAccessAttribute で RequiresSecureEndpoint プロパティを true に設定します。
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       https とドメイン サービスの併用に関する詳細については、MSDN を参照してください。

    /// <summary>
    /// ドメイン サービスはユーザーの登録を行います。
    /// </summary>
    [EnableClientAccess]
    public class UserRegistrationService : DomainService
    {
        /// <summary>
        /// ユーザーが既定で追加されるロールです。
        /// </summary>
        public const string DefaultRole = "Registered Users";

        //// メモ: これは、アプリケーションを起動するためのサンプル コードです。
        //// 運用コードでは、CAPTCHA 制御機能を指定するか、ユーザーの電子メール アドレスを確認することで、サービス拒否攻撃を軽減する必要があります。

        /// <summary>
        /// 指定された <see cref="RegistrationData"/> および <paramref name="password"/> を使用して新しいユーザーを追加します。
        /// </summary>
        /// <param name="user">このユーザーの登録情報です。</param>
        /// <param name="password">新しいユーザーのパスワードです。</param>
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

            // ロールが有効になり、既定のロールを使用できるように、ユーザーを作成する前にこれを実行します。
            //
            // ロール マネージャーに問題がある場合は、ユーザーを作成した後に失敗するよりも、この時点で失敗する方が適切です。
            if (!Roles.RoleExists(UserRegistrationService.DefaultRole))
            {
                Roles.CreateRole(UserRegistrationService.DefaultRole);
            }

            // メモ: 既定では、ASP.NET は SQL Server Express を使用してユーザー データベースを作成します。
            // SQL Server Express がインストールされていない場合、CreateUser は失敗します。
            MembershipCreateStatus createStatus;
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, true, null, out createStatus);

            if (createStatus != MembershipCreateStatus.Success)
            {
                return UserRegistrationService.ConvertStatus(createStatus);
            }

            // ユーザーを既定のロールに割り当てます。
            // ロール管理が無効になっている場合、これは失敗します。
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole);

            // フレンドリ名を設定します (プロファイル設定)。
            // web.config が正しく構成されていない場合、これは失敗します。
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
    /// <see cref="UserRegistrationService.CreateUser"/> から返される値の列挙です
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