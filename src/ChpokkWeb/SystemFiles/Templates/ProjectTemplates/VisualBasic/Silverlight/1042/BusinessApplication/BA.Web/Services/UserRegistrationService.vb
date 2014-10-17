Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: 응용 프로그램을 배포할 때 보안 끝점으로 전환합니다.
    '       https를 통해서만 사용자의 이름과 암호를 전달해야 합니다.
    '       이렇게 하려면 EnableClientAccessAttribute의 RequiresSecureEndpoint 속성을 true로 설정합니다.
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       도메인 서비스에서 https를 사용하는 방법에 대한 자세한 내용은 MSDN에서 확인할 수 있습니다.

    ''' <summary>
    ''' 도메인 서비스는 사용자 등록을 담당합니다.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' 사용자가 기본적으로 추가되는 역할입니다.
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' 참고: 응용 프로그램을 시작하는 샘플 코드입니다.
        ' 프로덕션 코드에서 CAPTCHA 컨트롤 기능을 제공하거나 사용자의 전자 메일 주소를 확인하여 서비스 거부 공격을 완화해야 합니다.

        ''' <summary>
        ''' 제공된 <see cref="RegistrationData"/> 및 <paramref name="password"/>를 가진 새 사용자를 추가합니다.
        ''' </summary>
        ''' <param name="user">이 사용자에 대한 등록 정보입니다.</param>
        ''' <param name="password">새 사용자의 암호입니다.</param>
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

            ' 사용자를 만들기 전에 실행하여 역할을 설정하고 기본 역할을 사용할 수 있는지 확인합니다.
            '
            ' 역할 관리자에 문제가 있는 경우 사용자를 만든 이후에 실패하는 것보다 지금 실패하는 것이 낫습니다.
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' 참고: 기본적으로 ASP.NET에서는 SQL Server Express를 사용하여 사용자 데이터베이스를 만듭니다. 
            ' SQL Server Express를 설치하지 않은 경우 CreateUser가 실패합니다.
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' 기본 역할에 할당
            ' 역할 관리를 사용하지 않도록 설정한 경우에만 *실패할 수 있습니다*.
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' 표시 이름 설정(프로필 설정)
            ' web.config를 잘못 구성한 경우에만 *실패할 수 있습니다*. 
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
    ''' <see cref="UserRegistrationService.CreateUser"/>에서 반환될 수 있는 값 열거형입니다.
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