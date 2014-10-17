Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Input
Imports $safeprojectname$.Controls

Namespace LoginUI
    ''' <summary>
    ''' Formulario que presenta los campos de inicio de sesión y controla el proceso de inicio de sesión.
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Crea una nueva instancia de <see cref="LoginForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Establezca el DataContext de este control en la instancia de LoginInfo para facilitar los enlaces.
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' Establece la ventana primaria del <see cref="LoginForm"/> actual.
        ''' </summary>
        ''' <param name="window">Ventana que se va a utilizar como primaria.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Controla el <see cref="DataForm.AutoGeneratingField"/> para proporcionar el PasswordAccessor.
        ''' </summary>
        Private Sub LoginForm_AutoGeneratingField(ByVal sender As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            If e.PropertyName = "UserName" Then
                Me.userNameTextBox = DirectCast(e.Field.Content, TextBox)
            ElseIf e.PropertyName = "Password" Then
                Dim passwordBox As New PasswordBox()
                e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty)
                loginInfo.PasswordAccessor = Function() passwordBox.Password
            End If
        End Sub

        ''' <summary>
        ''' Envía la <see cref="LoginOperation"/> al servidor
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' Es necesario forzar la validación, ya que no se está utilizando el botón Aceptar estándar del DataForm.
            ' Si no se garantiza que el formulario sea válido, se obtiene una excepción que invoca la operación en caso de que la entidad no sea válida.
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' Controlador de finalización de una <see cref="LoginOperation"/>.
        ''' Si la operación es correcta, cierra la ventana.
        ''' Si tiene un error, se muestra una <see cref="ErrorWindow"/> y se marca el error como controlado.
        ''' Si no se canceló pero se produjo un error de inicio de sesión, debe haber sido porque las credenciales eran incorrectas, así que se agrega un error de validación para notificar al usuario.
        ''' </summary>        
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If loginOperation.LoginSuccess Then
                parentWindow.DialogResult = True
            ElseIf loginOperation.HasError Then
                ErrorWindow.CreateNew(loginOperation.Error)
                loginOperation.MarkErrorAsHandled()
            ElseIf Not loginOperation.IsCanceled Then
                loginInfo.ValidationErrors.Add(New ValidationResult(ErrorResources.ErrorBadUserNameOrPassword, New String() {"UserName", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' Cambia al formulario de registro.
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' Si hay una operación de inicio de sesión en curso y se puede cancelar, hágalo.
        ''' De lo contrario, cierre la ventana.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Asigna Esc al botón Cancelar y Entrar al botón Aceptar.
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Establece el enfoque en el cuadro de texto de nombre de usuario.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace