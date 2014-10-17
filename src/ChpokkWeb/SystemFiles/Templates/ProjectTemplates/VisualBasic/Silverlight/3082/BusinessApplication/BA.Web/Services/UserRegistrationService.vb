Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: cambie a un extremo seguro al implementar la aplicación.
    '       El nombre de usuario y la contraseña solo se deben pasar mediante https.
    '       Para ello, establezca la propiedad RequiresSecureEndpoint de EnableClientAccessAttribute en true.
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       En MSDN encontrará más información sobre el uso de https con un servicio de dominio.

    ''' <summary>
    ''' Servicio de dominio responsable del registro de los usuarios.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' Rol al que se agregará a los usuarios de forma predeterminada.
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' NOTA: éste es un código de ejemplo para poner en marcha la aplicación.
        ' En el código de producción debería proporcionar una estrategia de reducción frente a un ataque de denegación de servicio al proporcionar funcionalidad de control CAPTCHA o comprobar la dirección de correo electrónico del usuario.

        ''' <summary>
        ''' Agrega un nuevo usuario con los <see cref="RegistrationData"/> y la <paramref name="password"/> proporcionados.
        ''' </summary>
        ''' <param name="user">Información de registro de este usuario.</param>
        ''' <param name="password">Contraseña del nuevo usuario.</param>
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

            ' Ejecute esto ANTES de crear el usuario para asegurarse de que los roles están habilitados y el rol predeterminado está disponible.
            '
            ' Si hay un problema con el administrador de roles, es mejor que se produzca un error ahora y no una vez creado el usuario.
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' NOTA: ASP.NET utiliza SQL Server Express de forma predeterminada para crear la base de datos de usuarios. 
            ' Si no tiene instalado SQL Server Express, se producirá un error de CreateUser.
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' Asignar al rol predeterminado
            ' Se *puede* producir un error, pero solo si la administración de roles está deshabilitada
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' Establecer su nombre descriptivo (establecimiento de perfil)
            ' Se *puede* producir un error, pero solo si web.config está configurado incorrectamente 
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
    ''' Enumeración de los valores que se pueden devolver desde <see cref="UserRegistrationService.CreateUser"/>
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