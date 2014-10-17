Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: переключиться на защищенную конечную точку при развертывании приложения.
    '       Имя и пароль пользователя должны передаваться только по протоколу https.
    '       Для этого нужно установить свойство RequiresSecureEndpoint элемента EnableClientAccessAttribute в значение true.
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       Дополнительные сведения об использовании протокола https в службе домена см. в MSDN.

    ''' <summary>
    ''' Служба домена отвечает за регистрацию пользователей.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' Роль, в которую пользователи будут добавляться по умолчанию.
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' ПРИМЕЧАНИЕ. Это образец кода, с которого можно начать разработку своего приложения.
        ' В коде для рабочей системы необходимо предусмотреть защиту от атак типа "отказ в обслуживании", добавив элемент управления CAPTCHA или проверку адреса электронной почты.

        ''' <summary>
        ''' Добавляет нового пользователя с указанными <see cref="RegistrationData"/> и <paramref name="password"/>.
        ''' </summary>
        ''' <param name="user">Регистрационные данные пользователя.</param>
        ''' <param name="password">Пароль нового пользователя.</param>
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

            ' Это нужно выполнить ДО создания пользователя, чтобы удостовериться, что нужные роли разрешены и доступна роль по умолчанию.
            '
            ' Если возникнет проблема с диспетчером ролей, то лучше выдать ошибку сейчас, чем после создания пользователя.
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' ПРИМЕЧАНИЕ. ASP.NET по умолчанию для создания базы данных пользователей использует SQL Server Express. 
            ' Вызов метода CreateUser завершится ошибкой, если не установлен SQL Server Express.
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' Назначить ролью по умолчанию
            ' Этот вызов *может* завершиться ошибкой, но только если отключено управление ролями
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' Задает понятное имя (параметр профиля)
            ' Этот вызов *может* завершиться ошибкой, но только если неверно настроен web.config 
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
    ''' Перечисление, содержащее значения, которые могут быть возвращены методом <see cref="UserRegistrationService.CreateUser"/>
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