Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: переключиться на защищенную конечную точку при развертывании приложения.
    ''       Имя и пароль пользователя должны передаваться только по протоколу https.
    ''       Для этого нужно установить свойство RequiresSecureEndpoint элемента EnableClientAccessAttribute в значение true.
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    '"       Дополнительные сведения об использовании протокола https в службе домена см. в MSDN.

    ''' <summary>
    ''' Служба домена отвечает за проверку подлинности пользователей при входе в приложение.
    '''
    ''' Большая часть функциональности уже реализована в классе AuthenticationBase.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace