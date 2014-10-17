namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: passare a un endpoint sicuro quando si distribuisce l'applicazione.
    //       La password e il nome dell'utente devono essere passati solo con https.
    //       A tale scopo, impostare la proprietà RequiresSecureEndpoint in EnableClientAccessAttribute su true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Ulteriori informazioni sull'utilizzo di https con un servizio del dominio sono disponibili su MSDN.

    /// <summary>
    /// Servizio del dominio responsabile dell'autenticazione degli utenti che accedono all'applicazione.
    ///
    /// La maggior parte delle funzionalità è già fornita dalla classe AuthenticationBase.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
