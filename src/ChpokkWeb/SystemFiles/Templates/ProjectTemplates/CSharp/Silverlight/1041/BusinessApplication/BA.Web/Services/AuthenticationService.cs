namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: アプリケーションを配置するときに安全なエンドポイントに切り替えます。
    //       ユーザーの名前とパスワードは、https のみを使用して渡す必要があります。
    //       そのためには、EnableClientAccessAttribute で RequiresSecureEndpoint プロパティを true に設定します。
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       https とドメイン サービスの併用に関する詳細については、MSDN を参照してください。

    /// <summary>
    /// ユーザーがアプリケーションにログオンするときに、ドメイン サービスはそのユーザーの認証を行います。
    ///
    /// ほとんどの機能は、AuthenticationBase クラスによって既に提供されています。
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
