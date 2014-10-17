Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: passare a un endpoint sicuro quando si distribuisce l'applicazione.
    ''       La password e il nome dell'utente devono essere passati solo con https.
    ''       A tale scopo, impostare la proprietà RequiresSecureEndpoint in EnableClientAccessAttribute su true.
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    ''       Ulteriori informazioni sull'utilizzo di https con un servizio del dominio sono disponibili su MSDN.

    ''' <summary>
    ''' Servizio del dominio responsabile dell'autenticazione degli utenti che accedono all'applicazione.
    '''
    ''' La maggior parte delle funzionalità è già fornita dalla classe AuthenticationBase.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace