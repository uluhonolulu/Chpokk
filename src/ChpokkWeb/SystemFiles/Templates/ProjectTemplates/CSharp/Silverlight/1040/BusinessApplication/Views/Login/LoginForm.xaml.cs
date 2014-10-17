namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Form che mostra i campi di accesso e gestisce il processo di accesso.
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// Crea una nuova istanza di <see cref="LoginForm"/>.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // Impostare l'oggetto DataContext di questo controllo sull'istanza di LoginInfo per consentirne facilmente l'associazione.
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// Imposta la finestra padre dell'oggetto <see cref="LoginForm"/> corrente.
        /// </summary>
        /// <param name="window">Finestra da utilizzare come elemento padre.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Gestisce <see cref="DataForm.AutoGeneratingField"/> per fornire l'oggetto PasswordAccessor.
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
        /// Invia l'oggetto <see cref="LoginOperation"/> al server
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // È necessario forzare la convalida poiché non viene utilizzato il pulsante OK standard dell'oggetto DataForm.
            // Se non si verifica la validità del form e l'entità non è valida, è possibile che venga visualizzata un'eccezione quando si richiama l'operazione.
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// Gestore completamento per un oggetto <see cref="LoginOperation"/>.
        /// Se l'operazione viene eseguita correttamente, la finestra viene chiusa.
        /// Se si verifica un errore, viene visualizzato un oggetto <see cref="ErrorWindow"/> e l'errore viene contrassegnato come gestito.
        /// Se l'operazione non è stata annullata ma l'accesso non è riuscito, è possibile che le credenziali non fossero corrette, pertanto viene aggiunto un errore di convalida per avvisare l'utente.
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
        /// Passa al form di registrazione.
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// Se è in corso un'operazione di accesso che può essere annullata, annullarla.
        /// In caso contrario, chiudere la finestra.
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
        /// Associa Esc al pulsante Annulla e Invio al pulsante OK.
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
        /// Imposta lo stato attivo sulla casella di testo del nome utente.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
