Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' ' TODO: Wechseln Sie beim Bereitstellen der Anwendung auf einen sicheren Endpunkt.
    '       Der Name und das Kennwort des Benutzers sollten ausschließlich mittels https übermittelt werden.
    '       Legen Sie dazu die Eigenschaft "RequiresSecureEndpoint" unter EnableClientAccessAttribute auf "true" fest.
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       Weitere Informationen zur Verwendung von https mit einem Domänendienst finden Sie auf MSDN.

    ''' <summary>
    ''' Domänendienst für das Registrieren von Benutzern.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' Rolle, der Benutzer standardmäßig hinzugefügt werden.
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' HINWEIS: Es handelt sich hierbei um Beispielcode, damit Sie Ihre Anwendung starten können.
        ' Im Code für die Produktion sollte eine Abwehr gegen einen Denial-of-Service-Angriff enthalten sein, z. B. durch Bereitstellung einer CAPTCHA-Funktionalität oder die Verifizierung der E-Mail-Adresse des Benutzers.

        ''' <summary>
        ''' Fügt einen neuen Benutzer mit den angegebenen <see cref="RegistrationData"/> und <paramref name="password"/> hinzu.
        ''' </summary>
        ''' <param name="user">Die Registrierungsinformationen für diesen Benutzer.</param>
        ''' <param name="password">Das Kennwort für den neuen Benutzer.</param>
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

            ' Dieser Vorgang sollte ausgeführt werden, BEVOR der Benutzer erstellt wird, um sicherzustellen, dass Rollen aktiviert sind und die Standardrolle verfügbar ist.
            '
            ' Falls es ein Problem mit dem Rollen-Manager gibt, ist es besser, wenn der Prozess vor dem Erstellen des Benutzers fehlschlägt als danach.
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' HINWEIS: ASP.NET nutzt standardmäßig SQL Server Express zum Erstellen der Benutzerdatenbank. 
            ' CreateUser schlägt fehl, wenn SQL Server Express nicht installiert ist.
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' Weisen Sie diesen der Standardrolle zu.
            ' Es *kann* sein, dass dieser Vorgang fehlschlägt, allerdings nur wenn die Rollenverwaltung deaktiviert ist.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' Weisen Sie den Anmeldenamen zu (Profileinstellung)
            ' Es *kann* sein, dass dieser Vorgang fehlschlägt, allerdings nur wenn "web.config" falsch konfiguriert ist. 
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
    ''' Eine Enumeration der Werte, die von <see cref="UserRegistrationService.CreateUser"/> zurückgegeben werden können
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