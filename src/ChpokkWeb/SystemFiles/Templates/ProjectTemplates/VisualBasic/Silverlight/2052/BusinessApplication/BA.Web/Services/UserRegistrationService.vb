Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: 在部署应用程序时切换为安全终结点。
    '       只应使用 https 传递用户名和密码。
    '       为此，请将 EnableClientAccessAttribute 的 RequiresSecureEndpoint 属性设置为 true。
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       有关对域服务使用 https 的更多信息可在 MSDN 上找到。

    ''' <summary>
    ''' 负责注册用户的域服务。
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' 默认情况下要将用户添加到的角色。
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' 注意: 这是可用于启动应用程序的示例代码。
        ' 在成品代码中，您应提供 CAPTCHA 控制功能或验证用户的电子邮件地址，从而针对拒绝服务攻击提供缓解措施。

        ''' <summary>
        ''' 使用提供的 <see cref="RegistrationData"/> 和 <paramref name="password"/> 添加新用户。
        ''' </summary>
        ''' <param name="user">此用户的注册信息。</param>
        ''' <param name="password">新用户的密码。</param>
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

            ' 请在创建用户之前运行，以确保角色启用且默认角色可用。
            '
            ' 如果角色管理器存在问题，则现在失败要好于在创建用户之后失败。
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' 注意: ASP.NET 默认情况下使用 SQL Server Express 创建用户数据库。
            ' 如果您未安装 SQL Server Express，则 CreateUser 会失败。
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' 将它分配给默认角色
            ' 这“可能”失败，但是仅当禁用角色管理时才会失败
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' 设置其友好名称(配置文件设置)
            ' 这“可能”失败，但是仅当 web.config 配置不正确时才会失败
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
    ''' 可以从 <see cref="UserRegistrationService.CreateUser"/> 返回的值的枚举
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