namespace $safeprojectname$.LoginUI
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using $safeprojectname$.Web;

    /// <summary>
    /// Form che mostra l'oggetto <see cref="RegistrationData"/> ed esegue il processo di registrazione.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// Crea una nuova istanza di <see cref="RegistrationForm"/>.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // Impostare l'oggetto DataContext di questo controllo sull'istanza di Registration per consentirne facilmente l'associazione.
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// Imposta la finestra padre dell'oggetto <see cref="RegistrationForm"/> corrente.
        /// </summary>
        /// <param name="window">Finestra da utilizzare come elemento padre.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Collegare le funzioni di accesso Password e PasswordConfirmation non appena vengono generati i campi.
        /// Associare inoltre il campo Question a un oggetto ComboBox contenente le domande di sicurezza e gestire l'evento LostFocus per l'oggetto TextBox di UserName.
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // Attivare la modalità di aggiunta per tutti i campi
            e.Field.Mode = DataFieldMode.AddNew;

            if (e.PropertyName == "UserName")
            {
                this.userNameTextBox = (TextBox)e.Field.Content;
                this.userNameTextBox.LostFocus += this.UserNameLostFocus;
            }
            else if (e.PropertyName == "Password")
            {
                PasswordBox passwordBox = new PasswordBox();
                e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty);
                this.registrationData.PasswordAccessor = () => passwordBox.Password;
            }
            else if (e.PropertyName == "PasswordConfirmation")
            {
                PasswordBox passwordConfirmationBox = new PasswordBox();
                e.Field.ReplaceTextBox(passwordConfirmationBox, PasswordBox.PasswordProperty);
                this.registrationData.PasswordConfirmationAccessor = () => passwordConfirmationBox.Password;
            }
            else if (e.PropertyName == "Question")
            {
                ComboBox questionComboBox = new ComboBox();
                questionComboBox.ItemsSource = RegistrationForm.GetSecurityQuestions();
                e.Field.ReplaceTextBox(questionComboBox, ComboBox.SelectedItemProperty, binding => binding.Converter = new TargetNullValueConverter());
            }
        }

        /// <summary>
        /// Callback per il momento in cui l'oggetto TextBox di UserName perde lo stato attivo.
        /// Chiamata nei dati di registrazione per consentire l'elaborazione della logica, possibilmente impostando il campo FriendlyName.
        /// </summary>
        /// <param name="sender">Mittente dell'evento.</param>
        /// <param name="e">Argomenti dell'evento.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// Restituisce un elenco di stringhe di risorse definite in <see cref="SecurityQuestions" />.
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // Utilizzare la reflection per recuperare tutte le domande di sicurezza localizzate
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// Inviare la nuova registrazione.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // È necessario forzare la convalida poiché non viene utilizzato il pulsante OK standard dell'oggetto DataForm.
            // Se non si verifica la validità del form e l'entità non è valida, è possibile che venga visualizzata un'eccezione quando si richiama l'operazione.
            if (this.registerForm.ValidateItem())
            {
                this.registrationData.CurrentOperation = this.userRegistrationContext.CreateUser(
                    this.registrationData,
                    this.registrationData.Password,
                    this.RegistrationOperation_Completed, null);

                this.parentWindow.AddPendingOperation(this.registrationData.CurrentOperation);
            }
        }

        /// <summary>
        /// Gestore completamento per l'operazione di registrazione. 
        /// Se si è verificato un errore, viene visualizzato un oggetto <see cref="ErrorWindow"/>.
        /// In caso contrario, viene attivata un'operazione di accesso che consentirà all'utente appena registrato di effettuare automaticamente l'accesso.
        /// </summary>
        private void RegistrationOperation_Completed(InvokeOperation<CreateUserStatus> operation)
        {
            if (!operation.IsCanceled)
            {
                if (operation.HasError)
                {
                    ErrorWindow.CreateNew(operation.Error);
                    operation.MarkErrorAsHandled();
                }
                else if (operation.Value == CreateUserStatus.Success)
                {
                    this.registrationData.CurrentOperation = WebContext.Current.Authentication.Login(this.registrationData.ToLoginParameters(), this.LoginOperation_Completed, null);
                    this.parentWindow.AddPendingOperation(this.registrationData.CurrentOperation);
                }
                else if (operation.Value == CreateUserStatus.DuplicateUserName)
                {
                    this.registrationData.ValidationErrors.Add(new ValidationResult(ErrorResources.CreateUserStatusDuplicateUserName, new string[] { "UserName" }));
                }
                else if (operation.Value == CreateUserStatus.DuplicateEmail)
                {
                    this.registrationData.ValidationErrors.Add(new ValidationResult(ErrorResources.CreateUserStatusDuplicateEmail, new string[] { "Email" }));
                }
                else
                {
                    ErrorWindow.CreateNew(ErrorResources.ErrorWindowGenericError);
                }
            }
        }

        /// <summary>
        /// Gestore completamento dell'operazione di accesso che si verifica dopo una corretta registrazione e un tentativo di accesso.
        /// La finestra verrà chiusa. Se l'operazione non riesce, nell'oggetto <see cref="ErrorWindow"/> verrà visualizzato il messaggio di errore.
        /// </summary>
        /// <param name="loginOperation">Oggetto <see cref="LoginOperation"/> completato.</param>
        private void LoginOperation_Completed(LoginOperation loginOperation)
        {
            if (!loginOperation.IsCanceled)
            {
                this.parentWindow.DialogResult = true;

                if (loginOperation.HasError)
                {
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, loginOperation.Error.Message));
                    loginOperation.MarkErrorAsHandled();
                }
                else if (loginOperation.LoginSuccess == false)
                {
                    // Sebbene l'operazione sia stata eseguita correttamente, l'accesso non è riuscito
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// Passa alla finestra di accesso.
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// Se è in corso un'operazione di registrazione o di accesso che è possibile annullare, annullarla.
        /// In caso contrario, chiudere la finestra.
        /// </summary>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            if (this.registrationData.CurrentOperation != null && this.registrationData.CurrentOperation.CanCancel)
            {
                this.registrationData.CurrentOperation.Cancel();
            }
            else
            {
                this.parentWindow.DialogResult = false;
            }
        }

        /// <summary>
        /// Associa Esc al pulsante Annulla e Invio al pulsante OK.
        /// </summary>
        private void RegistrationForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.CancelButton_Click(sender, e);
            }
            else if (e.Key == Key.Enter && this.registerButton.IsEnabled)
            {
                this.RegisterButton_Click(sender, e);
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
