Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: アプリケーションを配置するときに安全なエンドポイントに切り替えます。
    '       ユーザーの名前とパスワードは、https のみを使用して渡す必要があります。
    '       そのためには、EnableClientAccessAttribute で RequiresSecureEndpoint プロパティを true に設定します。
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       https とドメイン サービスの併用に関する詳細については、MSDN を参照してください。

    ''' <summary>
    ''' ドメイン サービスはユーザーの登録を行います。
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' ユーザーが既定で追加されるロールです。
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' メモ: これは、アプリケーションを起動するためのサンプル コードです。
        ' 運用コードでは、CAPTCHA 制御機能を指定するか、ユーザーの電子メール アドレスを確認することで、サービス拒否攻撃を軽減する必要があります。

        ''' <summary>
        ''' 指定された <see cref="RegistrationData"/> および <paramref name="password"/> を使用して新しいユーザーを追加します。
        ''' </summary>
        ''' <param name="user">このユーザーの登録情報です。</param>
        ''' <param name="password">新しいユーザーのパスワードです。</param>
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

            ' ロールが有効になり、既定のロールを使用できるように、ユーザーを作成する前にこれを実行します。
            '
            ' ロール マネージャーに問題がある場合は、ユーザーを作成した後に失敗するよりも、この時点で失敗する方が適切です。
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' メモ: 既定では、ASP.NET は SQL Server Express を使用してユーザー データベースを作成します。
            ' SQL Server Express がインストールされていない場合、CreateUser は失敗します。
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' 既定のロールに割り当てます
            ' これは、ロール管理が無効になっている場合にのみ失敗する可能性があります
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' フレンドリ名を設定します (プロファイル設定)
            ' これは、web.config が正しく構成されていない場合にのみ失敗する可能性があります
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
    ''' <see cref="UserRegistrationService.CreateUser"/> から返される値の列挙です
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