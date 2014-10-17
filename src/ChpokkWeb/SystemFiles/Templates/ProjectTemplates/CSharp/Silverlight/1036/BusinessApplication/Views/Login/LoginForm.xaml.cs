namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Formulaire qui présente les champs de connexion et gère le processus d'inscription.
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// Crée une nouvelle instance <see cref="LoginForm"/>.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // Affecter au DataContext de ce contrôle l'instance LoginInfo pour permettre une liaison facile.
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// Définit la fenêtre parente pour le <see cref="LoginForm"/> actuel.
        /// </summary>
        /// <param name="window">Fenêtre à utiliser comme parent.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Gère <see cref="DataForm.AutoGeneratingField"/> pour fournir le PasswordAccessor.
        /// </summary>
        private void LoginForm_AutoGeneratingField(object sender, DataFormAutoGeneratingFieldEventArgs e)
        {
            if (e.PropertyName == "UserName")
            {
                this.userNameTextBox = (TextBox)e.Field.Content;
            }
            else if (e.PropertyName == "Password")
            {
                PasswordBox passwordBox = new PasswordBox();
                e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty);
                this.loginInfo.PasswordAccessor = () => passwordBox.Password;
            }
        }

        /// <summary>
        /// Soumet le <see cref="LoginOperation"/> au serveur
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Nous devons forcer la validation car nous n'utilisons pas le bouton OK standard du DataForm.
            // Sans vérification de la validité du formulaire, nous obtenons une exception lors de l'appel de l'opération si l'entité n'est pas valide.
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// Gestionnaire d'achèvement d'un <see cref="LoginOperation"/>.
        /// Si l'opération a réussi, il ferme la fenêtre.
        /// S'il s'agit d'une erreur, il affiche un <see cref="ErrorWindow"/> et marque l'erreur comme gérée.
        /// Si elle n'a pas été annulée, mais que la connexion a échoué, il est probable que les informations d'identification étaient incorrectes ; une erreur de validation est donc ajoutée afin de le signaler à l'utilisateur.
        /// </summary>
        private void LoginOperation_Completed(LoginOperation loginOperation)
        {
            if (loginOperation.LoginSuccess)
            {
                this.parentWindow.DialogResult = true;
            }
            else if (loginOperation.HasError)
            {
                ErrorWindow.CreateNew(loginOperation.Error);
                loginOperation.MarkErrorAsHandled();
            }
            else if (!loginOperation.IsCanceled)
            {
                this.loginInfo.ValidationErrors.Add(new ValidationResult(ErrorResources.ErrorBadUserNameOrPassword, new string[] { "UserName", "Password" }));
            }
        }

        /// <summary>
        /// Bascule vers la fenêtre d'inscription.
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// Si une opération de connexion est en cours et qu'elle peut être annulée, annulez-la.
        /// Sinon, fermer la fenêtre.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (this.loginInfo.CurrentLoginOperation != null && this.loginInfo.CurrentLoginOperation.CanCancel)
            {
                this.loginInfo.CurrentLoginOperation.Cancel();
            }
            else
            {
                this.parentWindow.DialogResult = false;
            }
        }

        /// <summary>
        /// Mappe Esc au bouton d'annulation et Entrée au bouton OK.
        /// </summary>
        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.CancelButton_Click(sender, e);
            }
            else if (e.Key == Key.Enter && this.loginButton.IsEnabled)
            {
                this.LoginButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Définit le focus sur la zone de texte du nom d'utilisateur.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
