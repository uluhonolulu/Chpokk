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
    ''' Formular, das die Anmeldefelder bereitstellt und den Anmeldevorgang behandelt.
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Erstellt eine neue Instanz von <see cref="LoginForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Legen Sie den DataContext dieses Steuerelements auf die Instanz "LoginInfo" fest, um einfache Bindungen zuzulassen.
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' Legt das übergeordnete Fenster für das aktuelle <see cref="LoginForm"/> fest.
        ''' </summary>
        ''' <param name="window">Das Fenster, das als das übergeordnete Element verwendet werden soll.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Behandelt <see cref="DataForm.AutoGeneratingField"/>, um den PasswordAccessor bereitzustellen.
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
        ''' Sendet <see cref="LoginOperation"/> an den Server
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' Die Validierung muss erzwungen werden, da nicht die Standardschaltfläche "OK" aus DataForm verwendet wird.
            ' Wenn die Gültigkeit des Formulars nicht sichergestellt ist, wird sonst eine Ausnahme aufgerufen, die den Vorgang ausführt, falls die Entität ungültig ist.
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' Vervollständigungshandler für einen <see cref="LoginOperation"/>.
        ''' Falls der Vorgang erfolgreich ist, wird das Fenster geschlossen.
        ''' Falls der Vorgang fehlerhaft ist, wird ein <see cref="ErrorWindow"/> angezeigt, und der Fehler als behandelt markiert.
        ''' Falls der Vorgang nicht abgebrochen wurde, aber die Anmeldung fehlgeschlagen ist, muss dies daran gelegen haben, dass die Anmeldeinformationen fehlerhaft waren. Daher wird ein Validierungsfehler angefügt, um den Benutzer zu benachrichtigen.
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
        ''' Wechselt zum Registrierungsformular.
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' Falls ein Anmeldevorgang ausgeführt wird und dieser abgebrochen werden kann, brechen Sie diesen ab.
        ''' Schließen Sie andernfalls das Fenster.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Ordnet der Schaltfläche "Abbrechen" die Esc-Taste und der Schaltfläche "OK" die EINGABETASTE zu.
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
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