Imports System.ComponentModel.DataAnnotations

Namespace Web

    ''' <summary>
    ''' Classe contenant les valeurs et les règles de validation pour l'inscription de l'utilisateur.
    ''' </summary>
    Partial Public NotInheritable Class RegistrationData

        ''' <summary>
        ''' Obtient et définit le nom d'utilisateur.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=0, Name:="UserNameLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessageResourceName:="ValidationErrorInvalidUserName", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <StringLength(255, MinimumLength:=4, ErrorMessageResourceName:="ValidationErrorBadUserNameLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property UserName() As String

        ''' <summary>
        ''' Obtient et définit l'adresse de messagerie.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=2, Name:="EmailLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <RegularExpression("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName:="ValidationErrorInvalidEmail", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property Email() As String

        ''' <summary>
        ''' Obtient et définit le nom convivial de l'utilisateur.
        ''' </summary>
        <Display(Order:=1, Name:="FriendlyNameLabel", Description:="FriendlyNameDescription", ResourceType:=GetType(RegistrationDataResources))> _
        <StringLength(255, MinimumLength:=0, ErrorMessageResourceName:="ValidationErrorBadFriendlyNameLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property FriendlyName() As String

        ''' <summary>
        ''' Obtient et définit la question de sécurité.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=5, Name:="SecurityQuestionLabel", ResourceType:=GetType(RegistrationDataResources))> _
        Public Property Question() As String

        ''' <summary>
        ''' Obtient et définit la réponse à la question de sécurité.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=6, Name:="SecurityAnswerLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <StringLength(128, ErrorMessageResourceName:="ValidationErrorBadAnswerLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property Answer() As String
    End Class
End Namespace