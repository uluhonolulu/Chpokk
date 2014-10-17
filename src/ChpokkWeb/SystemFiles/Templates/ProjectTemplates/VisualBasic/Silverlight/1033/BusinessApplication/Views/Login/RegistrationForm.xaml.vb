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
    ''' Form that presents the <see cref="RegistrationData"/> and performs the registration process.
    ''' </summary>
    Partial Public Class RegistrationForm
        Inherits StackPanel

        Private parentWindow As LoginRegistrationWindow
        Private registrationData As New RegistrationData()
        Private userRegistrationContext As New UserRegistrationContext()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Creates a new <see cref="RegistrationForm"/> instance.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Set the DataContext of this control to the Registration instance to allow for easy binding.
            Me.DataContext = registrationData
        End Sub

        ''' <summary>
        ''' Sets the parent window for the current <see cref="RegistrationForm"/>.
        ''' </summary>
        ''' <param name="window">The window to use as the parent.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Wire up the Password and PasswordConfirmation accessors as the fields get generated.
        ''' Also bind the Question field to a ComboBox full of security questions, and handle the LostFocus event for the UserName TextBox.
        ''' </summary>
        Private Sub RegisterForm_AutoGeneratingField(ByVal dataForm As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            ' Put all the fields in adding mode
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
        ''' The callback for when the UserName TextBox loses focus.
        ''' Call into the registration data to allow logic to be processed, possibly setting the FriendlyName field.
        ''' </summary>
        ''' <param name="sender">The event sender.</param>
        ''' <param name="e">The event arguments.</param>
        Private Sub UserNameLostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
            registrationData.UserNameEntered(DirectCast(sender, TextBox).Text)
        End Sub

        ''' <summary>
        ''' Configure the specified binding to use a TargetNullValueConverter.
        ''' </summary>
        ''' <param name="comboBoxBinding">The binding to configure</param>
        Private Sub ConfigureComboBoxBinding(ByVal comboBoxBinding As Binding)
            comboBoxBinding.Converter = New TargetNullValueConverter()
        End Sub

        ''' <summary>
        ''' Returns a list of the resource strings defined in <see cref="SecurityQuestions" />.
        ''' </summary>
        Private Shared Function GetSecurityQuestions() As IEnumerable(Of String)
            ' Use reflection to grab all the localized security questions
            Return From propertyInfo In GetType(SecurityQuestions).GetProperties() _
                   Where propertyInfo.PropertyType.Equals(GetType(String)) _
                   Select DirectCast(propertyInfo.GetValue(Nothing, Nothing), String)
        End Function

        ''' <summary>
        ''' Submit the new registration.
        ''' </summary>
        Private Sub RegisterButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' We need to force validation since we are not using the standard OK button from the DataForm.
            ' Without ensuring the form is valid, we would get an exception invoking the operation if the entity is invalid.
            If Me.registerForm.ValidateItem() Then
                registrationData.CurrentOperation = userRegistrationContext.CreateUser(registrationData, registrationData.Password, AddressOf Me.RegistrationOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(registrationData.CurrentOperation)
            End If
        End Sub

        ''' <summary>
        ''' Completion handler for the registration operation. 
        ''' If there was an error, an <see cref="ErrorWindow"/> is displayed to the user.
        ''' Otherwise, this triggers a login operation that will automatically log in the just registered user.
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
        ''' Completion handler for the login operation that occurs after a successful registration and login attempt.  
        ''' This will close the window. If the operation fails, an <see cref="ErrorWindow"/> will display the error message.
        ''' </summary>
        ''' <param name="loginOperation">The <see cref="LoginOperation"/> that has completed.</param>
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If Not loginOperation.IsCanceled Then
                parentWindow.DialogResult = True

                If loginOperation.HasError Then
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, loginOperation.Error.Message))
                    loginOperation.MarkErrorAsHandled()
                ElseIf Not loginOperation.LoginSuccess Then
                    ' The operation was successful, but the actual login was not
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Switches to the login window.
        ''' </summary>
        Private Sub BackToLogin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' If a registration or login operation is in progress and is cancellable, cancel it.
        ''' Otherwise, close the window.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not IsNothing(registrationData.CurrentOperation)) AndAlso registrationData.CurrentOperation.CanCancel Then
                registrationData.CurrentOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Maps Esc to the cancel button and Enter to the OK button.
        ''' </summary>
        Private Sub RegistrationForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter And Me.registerButton.IsEnabled Then
                Me.RegisterButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Sets focus to the user name text box.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace