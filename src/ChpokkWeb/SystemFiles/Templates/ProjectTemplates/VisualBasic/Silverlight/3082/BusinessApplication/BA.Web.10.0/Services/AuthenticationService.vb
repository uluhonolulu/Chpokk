Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: cambie a un extremo seguro al implementar la aplicación.
    ''       El nombre de usuario y la contraseña solo se deben pasar mediante https.
    ''       Para ello, establezca la propiedad RequiresSecureEndpoint de EnableClientAccessAttribute en true.
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    ''       En MSDN encontrará más información sobre el uso de https con un servicio de dominio.

    ''' <summary>
    ''' Servicio de dominio responsable de la autenticación de los usuarios cuando inician sesión en la aplicación.
    '''
    ''' La clase AuthenticationBase ya proporciona la mayor parte de la funcionalidad.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace