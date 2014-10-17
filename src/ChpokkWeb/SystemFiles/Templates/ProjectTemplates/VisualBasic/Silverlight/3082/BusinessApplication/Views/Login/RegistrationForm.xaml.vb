Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Input
Imports $safeprojectname$.Web

Namespace LoginUI
    ''' <summary>
    ''' Formulario que presenta los <see cref="RegistrationData"/> y realiza el proceso de registro.
    ''' </summary>
    Partial Public Class RegistrationForm
        Inherits StackPanel

        Private parentWindow As LoginRegistrationWindow
        Private registrationData As New RegistrationData()
        Private userRegistrationContext As New UserRegistrationContext()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Crea una nueva instancia de <see cref="RegistrationForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Establezca el DataContext de este control en la instancia de Registration para facilitar los enlaces.
            Me.DataContext = registrationData
        End Sub

        ''' <summary>
        ''' Establece la ventana primaria del <see cref="RegistrationForm"/> actual.
        ''' </summary>
        ''' <param name="window">Ventana que se va a utilizar como primaria.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Aplique los descriptores de acceso Password y PasswordConfirmation a medida que los campos se generan.
        ''' Enlace además el campo Question a un ComboBox lleno de preguntas de seguridad y controle el evento LostFocus del TextBox UserName.
        ''' </summary>
        Private Sub RegisterForm_AutoGeneratingField(ByVal dataForm As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            ' Establecer todos los campos en modo de adición
            e.Field.Mode = DataFieldMode.AddNew

            If e.PropertyName = "UserName" Then
                Me.userNameTextBox = DirectCast(e.Field.Content, TextBox)
                AddHandler Me.userNameTextBox.LostFocus, AddressOf UserNameLostFocus
            ElseIf e.PropertyName = "Password" Then
                Dim passwordBox As New PasswordBox()
                e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty)
                registrationData.PasswordAccessor = Function() passwordBox.Password
            ElseIf e.PropertyName = "PasswordConfirmation" Then
                Dim passwordConfirmationBox As New PasswordBox()
                e.Field.ReplaceTextBox(passwordConfirmationBox, PasswordBox.PasswordProperty)
                registrationData.PasswordConfirmationAccessor = Function() passwordConfirmationBox.Password
            ElseIf e.PropertyName = "Question" Then
                Dim questionComboBox As New ComboBox()
                questionComboBox.ItemsSource = RegistrationForm.GetSecurityQuestions()
                e.Field.ReplaceTextBox(questionComboBox, ComboBox.SelectedItemProperty, AddressOf Me.ConfigureComboBoxBinding)
            End If
        End Sub

        ''' <summary>
        ''' Devolución de llamada para cuando el TextBox UserName pierde el enfoque.
        ''' Llamada a los datos de registro para permitir que se procese la lógica, posiblemente estableciendo el campo FriendlyName.
        ''' </summary>
        ''' <param name="sender">Remitente del evento.</param>
        ''' <param name="e">Argumentos del evento.</param>
        Private Sub UserNameLostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
            registrationData.UserNameEntered(DirectCast(sender, TextBox).Text)
        End Sub

        ''' <summary>
        ''' Configure el enlace especificado para utilizar un TargetNullValueConverter.
        ''' </summary>
        ''' <param name="comboBoxBinding">Es el binding que se va a configurar</param>
        Private Sub ConfigureComboBoxBinding(ByVal comboBoxBinding As Binding)
            comboBoxBinding.Converter = New TargetNullValueConverter()
        End Sub

        ''' <summary>
        ''' Devuelve una lista de las cadenas de recursos definidas en las <see cref="SecurityQuestions" />.
        ''' </summary>
        Private Shared Function GetSecurityQuestions() As IEnumerable(Of String)
            ' Utilizar reflection para obtener todas las preguntas de seguridad localizadas
            Return From propertyInfo In GetType(SecurityQuestions).GetProperties() _
                   Where propertyInfo.PropertyType.Equals(GetType(String)) _
                   Select DirectCast(propertyInfo.GetValue(Nothing, Nothing), String)
        End Function

        ''' <summary>
        ''' Envíe el nuevo registro.
        ''' </summary>
        Private Sub RegisterButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' Es necesario forzar la validación, ya que no se está utilizando el botón Aceptar estándar del DataForm.
            ' Si no se garantiza que el formulario sea válido, se obtiene una excepción que invoca la operación en caso de que la entidad no sea válida.
            If Me.registerForm.ValidateItem() Then
                registrationData.CurrentOperation = userRegistrationContext.CreateUser(registrationData, registrationData.Password, AddressOf Me.RegistrationOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(registrationData.CurrentOperation)
            End If
        End Sub

        ''' <summary>
        ''' Controlador de finalización de la operación de registro. 
        ''' Si se produjo un error, se muestra una <see cref="ErrorWindow"/> al usuario.
        ''' De lo contrario, desencadena una operación de inicio de sesión que automáticamente iniciará la sesión del usuario que se acaba de registrar.
        ''' </summary>
        Private Sub RegistrationOperation_Completed(ByVal operation As InvokeOperation(Of CreateUserStatus))
            If Not Operation.IsCanceled Then
                If operation.HasError Then
                    ErrorWindow.CreateNew(operation.Error)
                    operation.MarkErrorAsHandled()
                ElseIf operation.Value = CreateUserStatus.Success Then
                    registrationData.CurrentOperation = WebContext.Current.Authentication.Login(registrationData.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                    parentWindow.AddPendingOperation(registrationData.CurrentOperation)
                ElseIf operation.Value = CreateUserStatus.DuplicateUserName Then
                    registrationData.ValidationErrors.Add(New ValidationResult(ErrorResources.CreateUserStatusDuplicateUserName, New String() {"UserName"}))
                ElseIf operation.Value = CreateUserStatus.DuplicateEmail Then
                    registrationData.ValidationErrors.Add(New ValidationResult(ErrorResources.CreateUserStatusDuplicateEmail, New String() {"Email"}))
                Else
                    ErrorWindow.CreateNew(ErrorResources.ErrorWindowGenericError)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Controlador de finalización de la operación de inicio de sesión que se produce tras un intento de registro e inicio de sesión correcto.  
        ''' Esto cerrará la ventana. Si se produce un error en la operación, una <see cref="ErrorWindow"/> mostrará el mensaje de error.
        ''' </summary>
        ''' <param name="loginOperation"><see cref="LoginOperation"/> que se ha completado.</param>
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If Not loginOperation.IsCanceled Then
                parentWindow.DialogResult = True

                If loginOperation.HasError Then
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, loginOperation.Error.Message))
                    loginOperation.MarkErrorAsHandled()
                ElseIf Not loginOperation.LoginSuccess Then
                    ' La operación fue correcta, pero el inicio de sesión real no
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Cambia a la ventana de inicio de sesión.
        ''' </summary>
        Private Sub BackToLogin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Si hay una operación de registro o inicio de sesión en curso y se puede cancelar, hágalo.
        ''' De lo contrario, cierre la ventana.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not IsNothing(registrationData.CurrentOperation)) AndAlso registrationData.CurrentOperation.CanCancel Then
                registrationData.CurrentOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Asigna Esc al botón Cancelar y Entrar al botón Aceptar.
        ''' </summary>
        Private Sub RegistrationForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter And Me.registerButton.IsEnabled Then
                Me.RegisterButton_Click(sender, e)
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