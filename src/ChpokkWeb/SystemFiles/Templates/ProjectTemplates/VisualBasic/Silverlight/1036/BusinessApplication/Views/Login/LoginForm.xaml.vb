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
    ''' Formulaire qui présente les champs de connexion et gère le processus d'inscription.
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Crée une nouvelle instance <see cref="LoginForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Affecter au DataContext de ce contrôle l'instance LoginInfo pour permettre une liaison facile.
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' Définit la fenêtre parente pour le <see cref="LoginForm"/> actuel.
        ''' </summary>
        ''' <param name="window">Fenêtre à utiliser comme parent.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Gère <see cref="DataForm.AutoGeneratingField"/> pour fournir le PasswordAccessor.
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
        ''' Soumet le <see cref="LoginOperation"/> au serveur
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' Nous devons forcer la validation car nous n'utilisons pas le bouton OK standard du DataForm.
            ' Sans vérification de la validité du formulaire, nous obtenons une exception lors de l'appel de l'opération si l'entité n'est pas valide.
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' Gestionnaire d'achèvement d'un <see cref="LoginOperation"/>.
        ''' Si l'opération a réussi, il ferme la fenêtre.
        ''' S'il s'agit d'une erreur, nous affichons un <see cref="ErrorWindow"/> et marquons l'erreur comme gérée.
        ''' Si elle n'a pas été annulée, mais que la connexion a échoué, il est probable que les informations d'identification étaient incorrectes ; une erreur de validation est donc ajoutée afin de le signaler à l'utilisateur.
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
        ''' Bascule vers la fenêtre d'inscription.
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' Si une opération de connexion est en cours et qu'elle peut être annulée, annulez-la.
        ''' Sinon, fermer la fenêtre.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Mappe Esc au bouton d'annulation et Entrée au bouton OK.
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Définit le focus sur la zone de texte du nom d'utilisateur.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace