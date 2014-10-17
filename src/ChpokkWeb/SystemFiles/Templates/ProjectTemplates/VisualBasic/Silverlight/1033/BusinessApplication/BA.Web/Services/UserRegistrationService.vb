Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: Switch to a secure endpoint when deploying the application.
    '       The user's name and password should only be passed using https.
    '       To do this, set the RequiresSecureEndpoint property on EnableClientAccessAttribute to true.
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       More information on using https with a Domain Service can be found on MSDN.

    ''' <summary>
    ''' Domain Service responsible for registering users.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' Role to which users will be added by default.
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' NOTE: This is a sample code to get your application started.
        ' In the production code you should provide a mitigation against a denial of service attack by providing CAPTCHA control functionality or verifying user's email address.

        ''' <summary>
        ''' Adds a new user with the supplied <see cref="RegistrationData"/> and <paramref name="password"/>.
        ''' </summary>
        ''' <param name="user">The registration information for this user.</param>
        ''' <param name="password">The password for the new user.</param>
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

            ' Run this BEFORE creating the user to make sure roles are enabled and the default role is available.
            '
            ' If there is a problem with the role manager, it is better to fail now than to fail after the user is created.
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' NOTE: ASP.NET by default uses SQL Server Express to create the user database. 
            ' CreateUser will fail if you do not have SQL Server Express installed.
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' Assign it to the default role
            ' This *can* fail but only if role management is disabled
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' Set its friendly name (profile setting)
            ' This *can* fail but only if web.config is configured incorrectly 
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
    ''' An enumeration of the values that can be returned from <see cref="UserRegistrationService.CreateUser"/>
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