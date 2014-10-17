Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: passare a un endpoint sicuro quando si distribuisce l'applicazione.
    '       La password e il nome dell'utente devono essere passati solo con https.
    '       A tale scopo, impostare la proprietà RequiresSecureEndpoint in EnableClientAccessAttribute su true.
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       Ulteriori informazioni sull'utilizzo di https con un servizio del dominio sono disponibili su MSDN.

    ''' <summary>
    ''' Servizio del dominio responsabile della registrazione degli utenti.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' Ruolo a cui verranno aggiunti gli utenti per impostazione predefinita.
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' NOTA: si tratta di un codice di esempio per l'avvio dell'applicazione.
        ' Nel codice di produzione è necessario prevenire un attacco di tipo Denial of Service fornendo la funzionalità di controllo CAPTCHA o verificando l'indirizzo di posta elettronica dell'utente.

        ''' <summary>
        ''' Aggiunge un nuovo utente con gli oggetti <see cref="RegistrationData"/> e <paramref name="password"/> forniti.
        ''' </summary>
        ''' <param name="user">Informazioni di registrazione dell'utente.</param>
        ''' <param name="password">Password del nuovo utente.</param>
        <System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")> _
        <Invoke(HasSideEffects:=True)> _
        Public Function CreateUser(ByVal user As RegistrationData,
            <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
            <RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName:="ValidationErrorBadPasswordStrength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
            <StringLength(50, MinimumLength:=7, ErrorMessageResourceName:="ValidationErrorBadPasswordLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
            ByVal password As String) As CreateUserStatus
            If user Is Nothing Then
                Throw New ArgumentNullException("user")
            End If

            ' Eseguire questo metodo prima di creare l'utente per verificare che i ruoli siano abilitati e che il ruolo predefinito sia disponibile.
            '
            ' Se si verifica un problema con la gestione ruoli, è preferibile che l'operazione venga interrotta ora anziché dopo la creazione dell'utente.
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' NOTA: per impostazione predefinita, con ASP.NET viene utilizzato SQL Server Express per creare il database utente. 
            ' Impossibile eseguire CreateUser se non è installato SQL Server Express.
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' Assegnarlo al ruolo predefinito
            ' Può verificarsi un errore ma solo se la gestione ruoli è disabilitata
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' Impostare il relativo nome descrittivo (impostazione profilo)
            ' Può verificarsi un errore ma solo se web.config non è configurato correttamente 
            Dim profile As ProfileBase = ProfileBase.Create(user.UserName, True)
            profile.SetPropertyValue("FriendlyName", user.FriendlyName)
            profile.Save()

            Return CreateUserStatus.Success
        End Function

        Private Shared Function ConvertStatus(ByVal createStatus As MembershipCreateStatus) As CreateUserStatus
            Select Case createStatus
                Case MembershipCreateStatus.Success
                    Return CreateUserStatus.Success
                Case MembershipCreateStatus.InvalidPassword
                    Return CreateUserStatus.InvalidPassword
                Case MembershipCreateStatus.InvalidEmail
                    Return CreateUserStatus.InvalidEmail
                Case MembershipCreateStatus.InvalidAnswer
                    Return CreateUserStatus.InvalidAnswer
                Case MembershipCreateStatus.InvalidQuestion
                    Return CreateUserStatus.InvalidQuestion
                Case MembershipCreateStatus.InvalidUserName
                    Return CreateUserStatus.InvalidUserName
                Case MembershipCreateStatus.DuplicateUserName
                    Return CreateUserStatus.DuplicateUserName
                Case MembershipCreateStatus.DuplicateEmail
                    Return CreateUserStatus.DuplicateEmail
                Case Else
                    Return CreateUserStatus.Failure
            End Select
        End Function
    End Class

    ''' <summary>
    ''' Enumerazione dei valori che possono essere restituiti da <see cref="UserRegistrationService.CreateUser"/>
    ''' </summary>
    Public Enum CreateUserStatus
        Success = 0
        InvalidUserName = 1
        InvalidPassword = 2
        InvalidQuestion = 3
        InvalidAnswer = 4
        InvalidEmail = 5
        DuplicateUserName = 6
        DuplicateEmail = 7
        Failure = 8
    End Enum
End Namespace