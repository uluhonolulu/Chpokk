Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: Wechseln Sie beim Bereitstellen der Anwendung auf einen sicheren Endpunkt.
    ''       Der Name und das Kennwort des Benutzers sollten ausschließlich mittels https übermittelt werden.
    ''       Legen Sie dazu die Eigenschaft "RequiresSecureEndpoint" unter EnableClientAccessAttribute auf "true" fest.
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    ''       Weitere Informationen zur Verwendung von https mit einem Domänendienst finden Sie auf MSDN.

    ''' <summary>
    ''' Domänendienst für die Authentifizierung von Benutzern, wenn diese sich an der Anwendung anmelden.
    '''
    ''' Der Großteil der Funktionalität wird bereits durch die Klasse "AuthenticationBase" bereitgestellt.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace