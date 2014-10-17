Imports System.ComponentModel.DataAnnotations

Namespace Web

    ''' <summary>
    ''' 사용자 등록을 위한 값 및 유효성 검사 규칙을 포함하는 클래스입니다.
    ''' </summary>
    Partial Public NotInheritable Class RegistrationData

        ''' <summary>
        ''' 사용자 이름을 가져오거나 설정합니다.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=0, Name:="UserNameLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessageResourceName:="ValidationErrorInvalidUserName", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <StringLength(255, MinimumLength:=4, ErrorMessageResourceName:="ValidationErrorBadUserNameLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property UserName() As String

        ''' <summary>
        ''' 전자 메일 주소를 가져오거나 설정합니다.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=2, Name:="EmailLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <RegularExpression("^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessageResourceName:="ValidationErrorInvalidEmail", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property Email() As String

        ''' <summary>
        ''' 사용자의 표시 이름을 가져오거나 설정합니다.
        ''' </summary>
        <Display(Order:=1, Name:="FriendlyNameLabel", Description:="FriendlyNameDescription", ResourceType:=GetType(RegistrationDataResources))> _
        <StringLength(255, MinimumLength:=0, ErrorMessageResourceName:="ValidationErrorBadFriendlyNameLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property FriendlyName() As String

        ''' <summary>
        ''' 보안 질문을 가져오거나 설정합니다.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=5, Name:="SecurityQuestionLabel", ResourceType:=GetType(RegistrationDataResources))> _
        Public Property Question() As String

        ''' <summary>
        ''' 보안 질문에 대한 대답을 가져오거나 설정합니다.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=6, Name:="SecurityAnswerLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <StringLength(128, ErrorMessageResourceName:="ValidationErrorBadAnswerLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property Answer() As String
    End Class
End Namespace