namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: basculez vers un point de terminaison sécurisé lors du déploiement de l'application.
    //       Le nom et le mot de passe de l'utilisateur doivent être passés uniquement à l'aide de https.
    //       Pour ce faire, affectez à la propriété RequiresSecureEndpoint sur EnableClientAccessAttribute la valeur true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Pour plus d'informations sur l'utilisation de https avec un service de domaine, consultez MSDN.

    /// <summary>
    /// Service de domaine chargé de l'authentification des utilisateurs lorsqu'ils se connectent à l'application.
    ///
    /// La plupart des fonctionnalités sont déjà fournies par la classe AuthenticationBase.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
