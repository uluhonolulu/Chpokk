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
    ''' Formular, das die <see cref="RegistrationData"/> anzeigt und den Registrierungsvorgang durchführt.
    ''' </summary>
    Partial Public Class RegistrationForm
        Inherits StackPanel

        Private parentWindow As LoginRegistrationWindow
        Private registrationData As New RegistrationData()
        Private userRegistrationContext As New UserRegistrationContext()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Erstellt eine neue Instanz von <see cref="RegistrationForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Legen Sie den DataContext dieses Steuerelements auf die Instanz "Registration" fest, um einfache Bindungen zuzulassen.
            Me.DataContext = registrationData
        End Sub

        ''' <summary>
        ''' Legt das übergeordnete Fenster für das aktuelle <see cref="RegistrationForm"/> fest.
        ''' </summary>
        ''' <param name="window">Das Fenster, das als das übergeordnete Element verwendet werden soll.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Verknüpfen Sie den Password- und den PasswordConfirmation-Accessor beim Generieren der Felder.
        ''' Außerdem sollte das Feld "Question" an eine ComboBox mit Sicherheitsfragen gebunden werden und das Event "LostFocus" für die TextBox "UserName" behandelt werden.
        ''' </summary>
        Private Sub RegisterForm_AutoGeneratingField(ByVal dataForm As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            ' Alle Felder auf den Modus "hinzufügen" festlegen
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
        ''' Der Rückruf, wenn die TextBox "UserName" den Fokus verliert
        ''' Aufruf in die Registrierungsdaten, damit die Logik verarbeitet werden kann. Möglicherweise wird das Feld "FriendlyName" festgelegt.
        ''' </summary>
        ''' <param name="sender">Der Ereignissender.</param>
        ''' <param name="e">Die Ereignisargumente.</param>
        Private Sub UserNameLostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
            registrationData.UserNameEntered(DirectCast(sender, TextBox).Text)
        End Sub

        ''' <summary>
        ''' Konfigurieren der angegebenen Bindung für die Verwendung eines TargetNullValueConverter.
        ''' </summary>
        ''' <param name="comboBoxBinding">Die zu konfigurierende "binding"</param>
        Private Sub ConfigureComboBoxBinding(ByVal comboBoxBinding As Binding)
            comboBoxBinding.Converter = New TargetNullValueConverter()
        End Sub

        ''' <summary>
        ''' Gibt eine Liste der in <see cref="SecurityQuestions" /> definierten Ressourcenzeichenfolgen zurück.
        ''' </summary>
        Private Shared Function GetSecurityQuestions() As IEnumerable(Of String)
            ' Reflektion verwenden, um alle lokalisierten Sicherheitsfragen abzurufen
            Return From propertyInfo In GetType(SecurityQuestions).GetProperties() _
                   Where propertyInfo.PropertyType.Equals(GetType(String)) _
                   Select DirectCast(propertyInfo.GetValue(Nothing, Nothing), String)
        End Function

        ''' <summary>
        ''' Übermitteln der neuen Registrierung.
        ''' </summary>
        Private Sub RegisterButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' Die Validierung muss erzwungen werden, da nicht die Standardschaltfläche "OK" aus DataForm verwendet wird.
            ' Wenn die Gültigkeit des Formulars nicht sichergestellt ist, wird sonst eine Ausnahme aufgerufen, die den Vorgang ausführt, falls die Entität ungültig ist.
            If Me.registerForm.ValidateItem() Then
                registrationData.CurrentOperation = userRegistrationContext.CreateUser(registrationData, registrationData.Password, AddressOf Me.RegistrationOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(registrationData.CurrentOperation)
            End If
        End Sub

        ''' <summary>
        ''' Vervollständigungshandler für den Registrierungsvorgang. 
        ''' Wenn ein Fehler aufgetreten ist, wird dem Benutzer ein <see cref="ErrorWindow"/> angezeigt.
        ''' Andernfalls wird ein Anmeldevorgang ausgeführt, der den soeben registrierten Benutzer automatisch anmeldet.
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
        ''' Vervollständigungshandler für den Anmeldevorgang, der nach einer erfolgreichen Registrierung und einem Anmeldeversuch auftritt.  
        ''' Dadurch wird das Fenster geschlossen. Wenn der Vorgang fehlschlägt, wird die Fehlermeldung in einem <see cref="ErrorWindow"/> angezeigt.
        ''' </summary>
        ''' <param name="loginOperation">Der <see cref="LoginOperation"/>, der abgeschlossen wurde.</param>
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If Not loginOperation.IsCanceled Then
                parentWindow.DialogResult = True

                If loginOperation.HasError Then
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, loginOperation.Error.Message))
                    loginOperation.MarkErrorAsHandled()
                ElseIf Not loginOperation.LoginSuccess Then
                    ' Der Vorgang war erfolgreich, die tatsächliche Anmeldung dagegen nicht.
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Wechselt zum Anmeldefenster.
        ''' </summary>
        Private Sub BackToLogin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Falls ein Registrierungs- oder Anmeldevorgang ausgeführt wird und dieser abgebrochen werden kann, brechen Sie diesen ab.
        ''' Schließen Sie andernfalls das Fenster.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not IsNothing(registrationData.CurrentOperation)) AndAlso registrationData.CurrentOperation.CanCancel Then
                registrationData.CurrentOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Ordnet der Schaltfläche "Abbrechen" die Esc-Taste und der Schaltfläche "OK" die EINGABETASTE zu.
        ''' </summary>
        Private Sub RegistrationForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter And Me.registerButton.IsEnabled Then
                Me.RegisterButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Legt den Fokus auf das Textfeld "Benutzername" fest.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace