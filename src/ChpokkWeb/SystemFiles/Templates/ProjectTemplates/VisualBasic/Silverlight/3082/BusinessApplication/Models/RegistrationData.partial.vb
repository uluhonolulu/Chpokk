Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace Web

    Partial Public Class RegistrationData
        Private _CurrentOperation As OperationBase

        ''' <summary>
        ''' Obtiene o establece una función que devuelve la contraseña.
        ''' </summary>
        Friend Property PasswordAccessor() As Func(Of String)

        ''' <summary>
        ''' Obtiene y establece la contraseña.
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

                ' No almacene la contraseña en un campo privado: no se debe almacenar en memoria en texto sin formato.
                ' En su lugar, el PasswordAccessor proporcionado sirve como almacén de copia de seguridad del valor.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' Obtiene o establece una función que devuelve la confirmación de la contraseña.
        ''' </summary>
        Friend Property PasswordConfirmationAccessor() As Func(Of String)

        ''' <summary>
        ''' Obtiene y establece la cadena de confirmación de la contraseña.
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

                ' No almacene la contraseña en un campo privado: no se debe almacenar en memoria en texto sin formato.
                ' En su lugar, el PasswordAccessor proporcionado sirve como almacén de copia de seguridad del valor.

                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' Obtiene o establece la operación de registro o inicio de sesión actual.
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
        ''' Obtiene un valor que indica si el usuario se está registrando o iniciando sesión en ese momento.
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public ReadOnly Property IsRegistering() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentOperation)) AndAlso (Not Me.CurrentOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Método auxiliar para cuando cambia la operación actual.
        ''' Se utiliza para generar notificaciones de cambio de propiedades adecuadas.
        ''' </summary>
        ''' <param name="sender">Remitente del evento.</param>
        ''' <param name="e">Argumentos del evento.</param>
        Private Sub CurrentOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' Comprueba para asegurarse de que la contraseña y la confirmación coinciden.
        ''' Si no coinciden, se agrega un error de validación.
        ''' </summary>
        Private Sub CheckPasswordConfirmation()
            ' Si aún no se ha especificado alguna de las contraseñas, no pruebe la igualdad entre los campos.
            ' El atributo Required garantizará que se ha especificado un valor para ambos campos.
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Exit Sub
            End If

            ' Si los valores son distintos, agregue un error de validación con ambos miembros especificados.
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(New ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, New String() {"PasswordConfirmation", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' Realizar la lógica una vez especificado el valor UserName
        ''' </summary>
        ''' <param name="userName">Nombre de usuario especificado.</param>
        ''' <remarks>
        ''' Permita que el formulario indique si el valor se ha especificado totalmente.
        ''' El empleo del método OnUserNameChanged puede dar lugar a una llamada prematura antes de que el usuario haya terminado de especificar el valor en el formulario.
        ''' </remarks>
        Friend Sub UserNameEntered(ByVal userName As String)
            ' FriendlyName autorrellenado que coincide con el UserName de las nuevas entidades cuando no se ha especificado un nombre descriptivo
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If
        End Sub

        ''' <summary>
        ''' Crea un nuevo <see cref="LoginParameters"/> inicializado con los datos de esta entidad (IsPersistent se establecerá de forma predeterminada en false).
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function
    End Class
End Namespace