namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: 在部署应用程序时切换为安全终结点。
    //       只应使用 https 传递用户名和密码。
    //       为此，请将 EnableClientAccessAttribute 的 RequiresSecureEndpoint 属性设置为 true。
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       有关对域服务使用 https 的更多信息可在 MSDN 上找到。

    /// <summary>
    /// 负责在用户登录应用程序时对用户进行身份验证的域服务。
    ///
    /// AuthenticationBase 类已提供了大部分功能。
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
