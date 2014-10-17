namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // ' TODO: Wechseln Sie beim Bereitstellen der Anwendung auf einen sicheren Endpunkt.
    //       Der Name und das Kennwort des Benutzers sollten ausschließlich mittels https übermittelt werden.
    //       Legen Sie dazu die Eigenschaft "RequiresSecureEndpoint" unter EnableClientAccessAttribute auf "true" fest.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Weitere Informationen zur Verwendung von https mit einem Domänendienst finden Sie auf MSDN.

    /// <summary>
    /// Domänendienst für die Authentifizierung von Benutzern, wenn diese sich an der Anwendung anmelden.
    ///
    /// Der Großteil der Funktionalität wird bereits durch die Klasse "AuthenticationBase" bereitgestellt.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
