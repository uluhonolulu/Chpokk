Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: 部署此應用程式時切換到安全端點。
    '       使用者的名稱和密碼只可以使用 https 傳遞。
    '       若要執行此動作，請將 EnableClientAccessAttribute 上的 RequiresSecureEndpoint 屬性設定為 true。
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       可以在 MSDN 上取得搭配  https 使用網域服務的詳細資訊。

    ''' <summary>
    ''' 網域服務負責註冊使用者。
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' 使用者預設要加入的目標角色。
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' 注意: 這是讓應用程式啟動的範例程式碼。
        ' 在實際執行的程式碼中，您應該提供 CAPTCHA 控制功能或驗證使用者的電子郵件地址來緩和服務阻斷攻擊。

        ''' <summary>
        ''' 使用提供的 <see cref="RegistrationData"/> 和 <paramref name="password"/> 加入新的使用者。
        ''' </summary>
        ''' <param name="user">這位使用者的註冊資訊。</param>
        ''' <param name="password">新使用者的密碼。</param>
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

            ' 請於建立使用者之前執行，以確保角色會啟用，而且預設角色可供使用。
            '
            ' 如果角色管理員有問題，最好立即產生失敗，不要等到使用者建立之後才產生失敗。
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' 注意: 根據預設，ASP.NET 會使用 SQL Server Express 來建立使用者資料庫。
            ' 如果未安裝 SQL Server Express，CreateUser 將會失敗。
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' 將它指派到預設角色
            ' 它有可能失敗，但只會發生在停用角色管理的情況下
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' 設定它的易記名稱 (設定檔設定)
            ' 它有可能失敗，但只會發生在 web.config 設定不正確的情況下
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
    ''' 列舉可以從 <see cref="UserRegistrationService.CreateUser"/> 傳回的值
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