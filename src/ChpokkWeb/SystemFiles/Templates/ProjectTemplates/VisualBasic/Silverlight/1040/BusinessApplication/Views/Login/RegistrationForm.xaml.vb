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
    ''' Form che mostra l'oggetto <see cref="RegistrationData"/> ed esegue il processo di registrazione.
    ''' </summary>
    Partial Public Class RegistrationForm
        Inherits StackPanel

        Private parentWindow As LoginRegistrationWindow
        Private registrationData As New RegistrationData()
        Private userRegistrationContext As New UserRegistrationContext()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Crea una nuova istanza di <see cref="RegistrationForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Impostare l'oggetto DataContext di questo controllo sull'istanza di Registration per consentirne facilmente l'associazione.
            Me.DataContext = registrationData
        End Sub

        ''' <summary>
        ''' Imposta la finestra padre dell'oggetto <see cref="RegistrationForm"/> corrente.
        ''' </summary>
        ''' <param name="window">Finestra da utilizzare come elemento padre.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Collegare le funzioni di accesso Password e PasswordConfirmation non appena vengono generati i campi.
        ''' Associare inoltre il campo Question a un oggetto ComboBox contenente le domande di sicurezza e gestire l'evento LostFocus per l'oggetto TextBox di UserName.
        ''' </summary>
        Private Sub RegisterForm_AutoGeneratingField(ByVal dataForm As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            ' Attivare la modalità di aggiunta per tutti i campi
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
        ''' Callback per il momento in cui l'oggetto TextBox di UserName perde lo stato attivo.
        ''' Chiamata nei dati di registrazione per consentire l'elaborazione della logica, possibilmente impostando il campo FriendlyName.
        ''' </summary>
        ''' <param name="sender">Mittente dell'evento.</param>
        ''' <param name="e">Argomenti dell'evento.</param>
        Private Sub UserNameLostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
            registrationData.UserNameEntered(DirectCast(sender, TextBox).Text)
        End Sub

        ''' <summary>
        ''' Configurare l'associazione specificata in modo da utilizzare un oggetto TargetNullValueConverter.
        ''' </summary>
        ''' <param name="comboBoxBinding">Oggetto binding da configurare</param>
        Private Sub ConfigureComboBoxBinding(ByVal comboBoxBinding As Binding)
            comboBoxBinding.Converter = New TargetNullValueConverter()
        End Sub

        ''' <summary>
        ''' Restituisce un elenco di stringhe di risorse definite in <see cref="SecurityQuestions" />.
        ''' </summary>
        Private Shared Function GetSecurityQuestions() As IEnumerable(Of String)
            ' Utilizzare la reflection per recuperare tutte le domande di sicurezza localizzate
            Return From propertyInfo In GetType(SecurityQuestions).GetProperties() _
                   Where propertyInfo.PropertyType.Equals(GetType(String)) _
                   Select DirectCast(propertyInfo.GetValue(Nothing, Nothing), String)
        End Function

        ''' <summary>
        ''' Inviare la nuova registrazione.
        ''' </summary>
        Private Sub RegisterButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' È necessario forzare la convalida poiché non viene utilizzato il pulsante OK standard dell'oggetto DataForm.
            ' Se non si verifica la validità del form e l'entità non è valida, è possibile che venga visualizzata un'eccezione quando si richiama l'operazione.
            If Me.registerForm.ValidateItem() Then
                registrationData.CurrentOperation = userRegistrationContext.CreateUser(registrationData, registrationData.Password, AddressOf Me.RegistrationOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(registrationData.CurrentOperation)
            End If
        End Sub

        ''' <summary>
        ''' Gestore completamento per l'operazione di registrazione. 
        ''' Se si è verificato un errore, viene visualizzato un oggetto <see cref="ErrorWindow"/>.
        ''' In caso contrario, viene attivata un'operazione di accesso che consentirà all'utente appena registrato di effettuare automaticamente l'accesso.
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
        ''' Gestore completamento dell'operazione di accesso che si verifica dopo una corretta registrazione e un tentativo di accesso.  
        ''' La finestra verrà chiusa. Se l'operazione non riesce, nell'oggetto <see cref="ErrorWindow"/> verrà visualizzato il messaggio di errore.
        ''' </summary>
        ''' <param name="loginOperation">Oggetto <see cref="LoginOperation"/> completato.</param>
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If Not loginOperation.IsCanceled Then
                parentWindow.DialogResult = True

                If loginOperation.HasError Then
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, loginOperation.Error.Message))
                    loginOperation.MarkErrorAsHandled()
                ElseIf Not loginOperation.LoginSuccess Then
                    ' Sebbene l'operazione sia stata eseguita correttamente, l'accesso non è riuscito
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Passa alla finestra di accesso.
        ''' </summary>
        Private Sub BackToLogin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Se è in corso un'operazione di registrazione o di accesso che è possibile annullare, annullarla.
        ''' In caso contrario, chiudere la finestra.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not IsNothing(registrationData.CurrentOperation)) AndAlso registrationData.CurrentOperation.CanCancel Then
                registrationData.CurrentOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Associa Esc al pulsante Annulla e Invio al pulsante OK.
        ''' </summary>
        Private Sub RegistrationForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter And Me.registerButton.IsEnabled Then
                Me.RegisterButton_Click(sender, e)
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