namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Formular, das die Anmeldefelder bereitstellt und den Anmeldevorgang behandelt.
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="LoginForm"/>.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // Legen Sie den DataContext dieses Steuerelements auf die Instanz "LoginInfo" fest, um einfache Bindungen zuzulassen.
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// Legt das übergeordnete Fenster für das aktuelle <see cref="LoginForm"/> fest.
        /// </summary>
        /// <param name="window">Das Fenster, das als das übergeordnete Element verwendet werden soll.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Behandelt <see cref="DataForm.AutoGeneratingField"/>, um den PasswordAccessor bereitzustellen.
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
        /// Sendet <see cref="LoginOperation"/> an den Server
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Die Validierung muss erzwungen werden, da nicht die Standardschaltfläche "OK" aus DataForm verwendet wird.
            // Wenn die Gültigkeit des Formulars nicht sichergestellt ist, wird eine Ausnahme aufgerufen, die den Vorgang ausführt, falls die Entität ungültig ist.
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// Vervollständigungshandler für einen <see cref="LoginOperation"/>.
        /// Falls der Vorgang erfolgreich ist, wird das Fenster geschlossen.
        /// Falls der Vorgang fehlerhaft ist, wird ein <see cref="ErrorWindow"/> angezeigt, und der Fehler als behandelt markiert.
        /// Falls der Vorgang nicht abgebrochen wurde, aber die Anmeldung fehlgeschlagen ist, muss dies daran gelegen haben, dass die Anmeldeinformationen fehlerhaft waren. Daher wird ein Validierungsfehler angefügt, um den Benutzer zu benachrichtigen.
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
        /// Wechselt zum Registrierungsformular.
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// Falls ein Anmeldevorgang ausgeführt wird und dieser abgebrochen werden kann, brechen Sie diesen ab.
        /// Schließen Sie andernfalls das Fenster.
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
        /// Ordnet der Schaltfläche "Abbrechen" die Esc-Taste und der Schaltfläche "OK" die EINGABETASTE zu.
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
        /// Legt den Fokus auf das Textfeld "Benutzername" fest.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
