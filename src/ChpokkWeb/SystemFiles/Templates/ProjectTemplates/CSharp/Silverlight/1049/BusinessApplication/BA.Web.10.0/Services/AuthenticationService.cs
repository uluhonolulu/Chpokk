namespace $safeprojectname$
{
    using System.Security.Authentication;
    using System.ServiceModel.DomainServices.Hosting;
    using System.ServiceModel.DomainServices.Server;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;
    using System.Threading;

    // TODO: переключиться на защищенную конечную точку при развертывании приложения.
    //       Имя и пароль пользователя должны передаваться только по протоколу https.
    //       Для этого нужно установить свойство RequiresSecureEndpoint элемента EnableClientAccessAttribute в значение true.
    //   
    //       [EnableClientAccess(RequiresSecureEndpoint = true)]
    //
    //       Дополнительные сведения об использовании протокола https в службе домена см. в MSDN.

    /// <summary>
    /// Служба домена отвечает за проверку подлинности пользователей при входе в приложение.
    ///
    /// Большая часть функциональности уже реализована в классе AuthenticationBase.
    /// </summary>
    [EnableClientAccess]
    public class AuthenticationService : AuthenticationBase<User> { }
}
