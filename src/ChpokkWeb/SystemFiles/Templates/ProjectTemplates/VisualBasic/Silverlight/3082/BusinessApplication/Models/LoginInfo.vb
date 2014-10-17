Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace LoginUI
    ''' <summary>
    ''' Esta entidad interna se utiliza para facilitar el enlace entre los controles de la IU (DataForm y la etiqueta que muestra un error de validación) y las credenciales de inicio de sesión especificadas por el usuario.
    ''' </summary>
    Public Class LoginInfo
        Inherits ComplexObject
        Private _UserName As String

        ''' <summary>
        ''' Obtiene y establece el nombre de usuario.
        ''' </summary>
        <Display(Name:="UserNameLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <Required()> _
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._UserName, value) = False) Then
                    Me.ValidateProperty("UserName", value)
                    Me._UserName = value
                    Me.RaisePropertyChanged("UserName")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Obtiene o establece una función que devuelve la contraseña.
        ''' </summary>
        Friend Property PasswordAccessor As Func(Of String)

        ''' <summary>
        ''' Obtiene y establece la contraseña.
        ''' </summary>
        <Display(Name:="PasswordLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <Required()> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get
            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)

                ' No almacene la contraseña en un campo privado: no se debe almacenar en memoria en texto sin formato.
                ' En su lugar, el PasswordAccessor proporcionado sirve como almacén de copia de seguridad del valor.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        Private _RememberMe As Boolean

        ''' <summary>
        ''' Obtiene y establece el valor que indica si se debe registrar la información de autenticación del usuario para futuros inicios de sesión.
        ''' </summary>
        <Display(Name:="RememberMeLabel", ResourceType:=GetType(ApplicationStrings))> _
        Public Property RememberMe() As Boolean
            Get
                Return _RememberMe
            End Get
            Set(ByVal value As Boolean)
                If _RememberMe <> value Then
                    Me.ValidateProperty("RememberMe", value)
                    Me._RememberMe = value
                    Me.RaisePropertyChanged("RememberMe")
                End If
            End Set
        End Property

        Private _CurrentLoginOperation As LoginOperation

        ''' <summary>
        ''' Obtiene o establece la operación de inicio de sesión actual.
        ''' </summary>
        Friend Property CurrentLoginOperation() As LoginOperation
            Get
                Return _CurrentLoginOperation
            End Get
            Set(ByVal value As LoginOperation)
                If Not Object.Equals(_CurrentLoginOperation, value) Then
                    If Not IsNothing(_CurrentLoginOperation) Then
                        RemoveHandler _CurrentLoginOperation.Completed, AddressOf Me.CurrentLoginOperationChanged
                    End If

                    _CurrentLoginOperation = value

                    If Not IsNothing(_CurrentLoginOperation) Then
                        AddHandler _CurrentLoginOperation.Completed, AddressOf Me.CurrentLoginOperationChanged
                    End If

                    Me.CurrentLoginOperationChanged(Me, EventArgs.Empty)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Obtiene un valor que indica si el usuario está iniciando sesión en ese momento.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property IsLoggingIn() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentLoginOperation)) AndAlso (Not Me.CurrentLoginOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Obtiene un valor que indica si el usuario puede iniciar sesión en ese momento.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property CanLogIn As Boolean
            Get
                Return (Not Me.IsLoggingIn)
            End Get
        End Property

        ''' <summary>
        ''' Genera notificaciones de cambio de propiedades relacionadas con la operación cuando cambia la operación actual de inicio de sesión.
        ''' </summary>
        Private Sub CurrentLoginOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsLoggingIn")
            Me.RaisePropertyChanged("CanLogIn")
        End Sub

        ''' <summary>
        ''' Crea un nueva instancia de <see cref="LoginParameters"/> con los datos almacenados en esta entidad.
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, Me.RememberMe, Nothing)
        End Function
    End Class
End Namespace