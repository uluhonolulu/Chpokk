Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace Web

    Partial Public Class RegistrationData
        Private _CurrentOperation As OperationBase

        ''' <summary>
        ''' 암호를 반환하는 함수를 가져오거나 설정합니다.
        ''' </summary>
        Friend Property PasswordAccessor() As Func(Of String)

        ''' <summary>
        ''' 암호를 가져오거나 설정합니다.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=3, Name:="PasswordLabel", Description:="PasswordDescription", ResourceType:=GetType(RegistrationDataResources))> _
        <RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName:="ValidationErrorBadPasswordStrength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <StringLength(50, MinimumLength:=7, ErrorMessageResourceName:="ValidationErrorBadPasswordLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get

            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)
                Me.CheckPasswordConfirmation()

                ' 암호를 메모리에 일반 텍스트로 저장해서는 안 되므로 암호를 전용 필드에 저장하지 마십시오.
                ' 대신, 제공된 PasswordAccessor가 값에 대한 백업 저장소 역할을 합니다.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' 암호 확인을 반환하는 함수를 가져오거나 설정합니다.
        ''' </summary>
        Friend Property PasswordConfirmationAccessor() As Func(Of String)

        ''' <summary>
        ''' 암호 확인 문자열을 가져오거나 설정합니다.
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=4, Name:="PasswordConfirmationLabel", ResourceType:=GetType(RegistrationDataResources))> _
        Public Property PasswordConfirmation() As String
            Get
                Return If((Me.PasswordConfirmationAccessor Is Nothing), String.Empty, Me.PasswordConfirmationAccessor.Invoke())
            End Get

            Set(ByVal value As String)
                Me.ValidateProperty("PasswordConfirmation", value)
                Me.CheckPasswordConfirmation()

                ' 암호를 메모리에 일반 텍스트로 저장해서는 안 되므로 암호를 전용 필드에 저장하지 마십시오.
                ' 대신, 제공된 PasswordAccessor가 값에 대한 백업 저장소 역할을 합니다.

                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' 현재 등록 또는 로그인 작업을 가져오거나 설정합니다.
        ''' </summary>
        Friend Property CurrentOperation() As OperationBase
            Get
                Return _CurrentOperation
            End Get
            Set(ByVal value As OperationBase)
                If Not Object.Equals(_CurrentOperation, value) Then
                    If Not IsNothing(_CurrentOperation) Then
                        RemoveHandler _CurrentOperation.Completed, AddressOf Me.CurrentOperationChanged
                    End If

                    _CurrentOperation = value

                    If Not IsNothing(_CurrentOperation) Then
                        AddHandler _CurrentOperation.Completed, AddressOf Me.CurrentOperationChanged
                    End If

                    Me.CurrentOperationChanged(Me, EventArgs.Empty)
                End If
            End Set
        End Property

        ''' <summary>
        ''' 사용자가 현재 등록되어 있거나 로그인해 있는지 여부를 나타내는 값을 가져옵니다.
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public ReadOnly Property IsRegistering() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentOperation)) AndAlso (Not Me.CurrentOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' 현재 작업이 변경된 경우에 대한 도우미 메서드입니다.
        ''' 적절한 속성 변경 알림을 표시하는 데 사용됩니다.
        ''' </summary>
        ''' <param name="sender">이벤트 전송자입니다.</param>
        ''' <param name="e">event 인수입니다.</param>
        Private Sub CurrentOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' 암호와 암호 확인이 일치하는지 확인합니다.
        ''' 일치하지 않는 경우 유효성 검사 오류가 추가됩니다.
        ''' </summary>
        Private Sub CheckPasswordConfirmation()
            ' 암호 또는 암호 확인을 입력하지 않은 경우 두 필드가 일치하는지 테스트하지 않습니다.
            ' Required 특성은 두 필드 모두에 값이 입력되었는지 확인합니다.
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Exit Sub
            End If

            ' 값이 다른 경우 두 멤버를 지정하여 유효성 검사 오류를 추가합니다.
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(New ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, New String() {"PasswordConfirmation", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' UserName 값이 입력된 이후에 논리를 수행합니다.
        ''' </summary>
        ''' <param name="userName">입력된 사용자 이름입니다.</param>
        ''' <remarks>
        ''' 폼을 사용하여 값이 입력된 시간을 나타낼 수 있습니다.
        ''' OnUserNameChanged 메서드를 사용하면 사용자가 폼에 값을 입력하기 전에 호출이 발생할 수 있습니다.
        ''' </remarks>
        Friend Sub UserNameEntered(ByVal userName As String)
            ' 표시 이름을 지정하지 않은 경우 새 엔터티에 대한 UserName과 일치하도록 FriendlyName을 자동으로 채웁니다.
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If
        End Sub

        ''' <summary>
        ''' 이 엔터티의 데이터로 초기화되는 새 <see cref="LoginParameters"/>를 만듭니다. IsPersistent의 기본값은 false입니다.
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function
    End Class
End Namespace