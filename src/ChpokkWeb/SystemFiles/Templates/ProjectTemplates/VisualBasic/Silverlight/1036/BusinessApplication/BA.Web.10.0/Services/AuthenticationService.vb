Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: basculez vers un point de terminaison sécurisé lors du déploiement de l'application.
    ''       Le nom et le mot de passe de l'utilisateur doivent être passés uniquement à l'aide de https.
    ''       Pour ce faire, affectez à la propriété RequiresSecureEndpoint sur EnableClientAccessAttribute la valeur true.
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    ''       Pour plus d'informations sur l'utilisation de https avec un service de domaine, consultez MSDN.

    ''' <summary>
    ''' Service de domaine chargé de l'authentification des utilisateurs lorsqu'ils se connectent à l'application.
    '''
    ''' La plupart des fonctionnalités sont déjà fournies par la classe AuthenticationBase.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace