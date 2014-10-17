Imports System
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.Web.Profile
Imports System.Web.Security
Namespace Web

    ' TODO: basculez vers un point de terminaison sécurisé lors du déploiement de l'application.
    '       Le nom et le mot de passe de l'utilisateur doivent être passés uniquement à l'aide de https.
    '       Pour ce faire, affectez à la propriété RequiresSecureEndpoint sur EnableClientAccessAttribute la valeur true.
    '
    '       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    '
    '       Pour plus d'informations sur l'utilisation de https avec un service de domaine, consultez MSDN.

    ''' <summary>
    ''' Service de domaine chargé de l'inscription des utilisateurs.
    ''' </summary>
    <EnableClientAccess()> _
    Public Class UserRegistrationService
        Inherits DomainService

        ''' <summary>
        ''' Rôle auquel les utilisateurs seront ajoutés par défaut.
        ''' </summary>
        Public Const DefaultRole As String = "Registered Users"

        ' REMARQUE : Ceci est un exemple de code permettant de faire démarrer votre application.
        ' Dans le code de production, vous devez fournir une atténuation d'une attaque par déni de service en fournissant la fonctionnalité de contrôle CAPTCHA ou en vérifiant l'adresse de messagerie de l'utilisateur.

        ''' <summary>
        ''' Ajoute un nouvel utilisateur avec les <see cref="RegistrationData"/> et <paramref name="password"/> fournis.
        ''' </summary>
        ''' <param name="user">Informations d'inscription de cet utilisateur.</param>
        ''' <param name="password">Mot de passe du nouvel utilisateur.</param>
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

            ' Effectuer cette exécution AVANT de créer l'utilisateur pour vérifier que les rôles sont activés et que le rôle par défaut est disponible.
            '
            ' Si le gestionnaire de rôles présente un problème, il est préférable que l'échec se produise maintenant plutôt qu'après la création de l'utilisateur.
            If Not Roles.RoleExists(UserRegistrationService.DefaultRole) Then
                Roles.CreateRole(UserRegistrationService.DefaultRole)
            End If

            ' REMARQUE : ASP.NET utilise par défaut SQL Server Express pour créer la base de données utilisateur. 
            ' CreateUser échoue si SQL Server Express n'est pas installé.
            Dim createStatus As MembershipCreateStatus
            Membership.CreateUser(user.UserName, password, user.Email, user.Question, user.Answer, True, Nothing, createStatus)

            If createStatus <> MembershipCreateStatus.Success Then
                Return UserRegistrationService.ConvertStatus(createStatus)
            End If

            ' L'assigner au rôle par défaut
            ' Cette opération *peut* échouer, mais uniquement si la gestion des rôles est désactivée
            Roles.AddUserToRole(user.UserName, UserRegistrationService.DefaultRole)

            ' Définir son nom convivial (paramètre de profil)
            ' Cette opération *peut* échouer, mais uniquement si web.config n'est pas configuré correctement 
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
    ''' Énumération des valeurs qui peuvent être retournées par <see cref="UserRegistrationService.CreateUser"/>
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