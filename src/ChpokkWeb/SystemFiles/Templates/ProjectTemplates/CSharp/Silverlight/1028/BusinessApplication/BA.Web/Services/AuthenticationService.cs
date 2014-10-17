namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: 部署此應用程式時切換到安全端點。
    //       使用者的名稱和密碼只可以使用 https 傳遞。
    //       若要執行此動作，請將 EnableClientAccessAttribute 上的 RequiresSecureEndpoint 屬性設定為 true。
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       可以在 MSDN 上取得搭配  https 使用網域服務的詳細資訊。

    /// <summary>
    /// 網域服務負責在使用者登入應用程式時對使用者進行驗證。
    ///
    /// 大部分的功能已由 AuthenticationBase 類別提供。
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
