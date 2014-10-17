Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace Web

    Partial Public Class RegistrationData
        Private _CurrentOperation As OperationBase

        ''' <summary>
        ''' Возвращает или задает функцию, возвращающую пароль.
        ''' </summary>
        Friend Property PasswordAccessor() As Func(Of String)

        ''' <summary>
        ''' Возвращает и задает пароль.
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

                ' Не сохраняйте пароль в скрытых полях, так как он не должен храниться в памяти в виде обычного текста.
                ' Вместо этого для сохранения значения используйте объект PasswordAccessor.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' Возвращает или задает функцию, возвращающую подтверждение пароля.
        ''' </summary>
        Friend Property PasswordConfirmationAccessor() As Func(Of String)

        ''' <summary>
        ''' Возвращает и задает строку подтверждения пароля.
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

                ' Не сохраняйте пароль в скрытых полях, так как он не должен храниться в памяти в виде обычного текста.
                ' Вместо этого для сохранения значения используйте объект PasswordAccessor.

                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' Возвращает или задает текущую операцию регистрации или входа.
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
        ''' Возвращает значение, указывающее, производит ли пользователь в настоящее время регистрацию или вход.
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public ReadOnly Property IsRegistering() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentOperation)) AndAlso (Not Me.CurrentOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Вспомогательный метод для изменения текущей операции.
        ''' Служит для вызова соответствующих уведомлений об изменении свойств.
        ''' </summary>
        ''' <param name="sender">Отправитель события.</param>
        ''' <param name="e">Аргументы события (event).</param>
        Private Sub CurrentOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' Проверяет пароль на соответствие подтверждению пароля.
        ''' Если нет совпадения, добавляется ошибка проверки.
        ''' </summary>
        Private Sub CheckPasswordConfirmation()
            ' Если пароль или подтверждение не введены, эти поля на равенство не проверяются.
            ' Атрибут Required обеспечивает ввод значений в обоих полях.
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Exit Sub
            End If

            ' Если значения различаются, добавляется ошибка проверки, связанная с обоими указанными элементами.
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(New ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, New String() {"PasswordConfirmation", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' Выполняет логику после ввода значения UserName
        ''' </summary>
        ''' <param name="userName">Введенное имя пользователя.</param>
        ''' <remarks>
        ''' Позволяет форме указывать на неполный ввод значения.
        ''' Использование метода OnUserNameChanged может привести к преждевременному вызову, прежде чем пользователь завершит ввод значения в форме.
        ''' </remarks>
        Friend Sub UserNameEntered(ByVal userName As String)
            ' Автозаполнение FriendlyName для новых сущностей, соответствующего UserName, когда понятное пользователю имя не указано
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If
        End Sub

        ''' <summary>
        ''' Создает новый объект <see cref="LoginParameters"/> и инициализирует его данными этой сущности (свойство IsPersistent по умолчанию имеет значение false).
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function
    End Class
End Namespace