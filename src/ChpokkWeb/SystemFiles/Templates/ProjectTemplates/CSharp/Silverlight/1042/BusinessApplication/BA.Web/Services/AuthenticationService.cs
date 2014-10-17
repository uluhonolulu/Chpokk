namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: 응용 프로그램을 배포할 때 보안 끝점으로 전환합니다.
    //       https를 통해서만 사용자의 이름과 암호를 전달해야 합니다.
    //       이렇게 하려면 EnableClientAccessAttribute의 RequiresSecureEndpoint 속성을 true로 설정합니다.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       도메인 서비스에서 https를 사용하는 방법에 대한 자세한 내용은 MSDN에서 확인할 수 있습니다.

    /// <summary>
    /// 도메인 서비스는 사용자가 응용 프로그램에 로그온할 때 해당 사용자를 인증합니다.
    ///
    /// 대부분의 기능은 AuthenticationBase 클래스에서 이미 제공하고 있습니다.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
