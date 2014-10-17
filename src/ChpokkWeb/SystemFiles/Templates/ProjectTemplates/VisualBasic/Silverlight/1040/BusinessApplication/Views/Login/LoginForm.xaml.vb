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
    ''' Form che mostra i campi di accesso e gestisce il processo di accesso.
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Crea una nuova istanza di <see cref="LoginForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Impostare l'oggetto DataContext di questo controllo sull'istanza di LoginInfo per consentirne facilmente l'associazione.
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' Imposta la finestra padre dell'oggetto <see cref="LoginForm"/> corrente.
        ''' </summary>
        ''' <param name="window">Finestra da utilizzare come elemento padre.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Gestisce <see cref="DataForm.AutoGeneratingField"/> per fornire l'oggetto PasswordAccessor.
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
        ''' Invia l'oggetto <see cref="LoginOperation"/> al server
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' È necessario forzare la convalida poiché non viene utilizzato il pulsante OK standard dell'oggetto DataForm.
            ' Se non si verifica la validità del form e l'entità non è valida, è possibile che venga visualizzata un'eccezione quando si richiama l'operazione.
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' Gestore completamento per un oggetto <see cref="LoginOperation"/>.
        ''' Se l'operazione viene eseguita correttamente, la finestra viene chiusa.
        ''' Se si verifica un errore, viene visualizzato un oggetto <see cref="ErrorWindow"/> e l'errore viene contrassegnato come gestito.
        ''' Se l'operazione non è stata annullata ma l'accesso non è riuscito, è possibile che le credenziali non fossero corrette, pertanto viene aggiunto un errore di convalida per avvisare l'utente.
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
        ''' Passa al form di registrazione.
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' Se è in corso un'operazione di accesso che può essere annullata, annullarla.
        ''' In caso contrario, chiudere la finestra.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Associa Esc al pulsante Annulla e Invio al pulsante OK.
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Imposta lo stato attivo sulla casella di testo del nome utente.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace