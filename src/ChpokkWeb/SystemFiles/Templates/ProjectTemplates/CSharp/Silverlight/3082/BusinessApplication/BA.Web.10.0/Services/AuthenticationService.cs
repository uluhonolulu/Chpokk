namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: cambie a un extremo seguro al implementar la aplicación.
    //       El nombre de usuario y la contraseña solo se deben pasar mediante https.
    //       Para ello, establezca la propiedad RequiresSecureEndpoint de EnableClientAccessAttribute en true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       En MSDN encontrará más información sobre el uso de https con un servicio de dominio.

    /// <summary>
    /// Servicio de dominio responsable de la autenticación de los usuarios cuando inician sesión en la aplicación.
    ///
    /// La clase AuthenticationBase ya proporciona la mayor parte de la funcionalidad.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
