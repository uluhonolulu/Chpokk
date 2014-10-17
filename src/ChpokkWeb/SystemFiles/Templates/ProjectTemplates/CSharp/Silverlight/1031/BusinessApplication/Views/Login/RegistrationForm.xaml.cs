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
    /// Formular, das die <see cref="RegistrationData"/> anzeigt und den Registrierungsvorgang durchführt.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="RegistrationForm"/>.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // Legen Sie den DataContext dieses Steuerelements auf die Instanz "Registration" fest, um einfache Bindungen zuzulassen.
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// Legt das übergeordnete Fenster für das aktuelle <see cref="RegistrationForm"/> fest.
        /// </summary>
        /// <param name="window">Das Fenster, das als das übergeordnete Element verwendet werden soll.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Verknüpfen Sie den Password- und den PasswordConfirmation-Accessor beim Generieren der Felder.
        /// Außerdem sollte das Feld "Question" an eine ComboBox mit Sicherheitsfragen gebunden werden und das Event "LostFocus" für die TextBox "UserName" behandelt werden.
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // Alle Felder auf den Modus "hinzufügen" festlegen
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
        /// Der Rückruf, wenn die TextBox "UserName" den Fokus verliert
        /// Aufruf in die Registrierungsdaten, damit die Logik verarbeitet werden kann. Möglicherweise wird das Feld "FriendlyName" festgelegt.
        /// </summary>
        /// <param name="sender">Der Ereignissender.</param>
        /// <param name="e">Die Ereignisargumente.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// Gibt eine Liste der in <see cref="SecurityQuestions" /> definierten Ressourcenzeichenfolgen zurück.
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // Reflektion verwenden, um alle lokalisierten Sicherheitsfragen abzurufen
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// Übermitteln der neuen Registrierung.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Die Validierung muss erzwungen werden, da nicht die Standardschaltfläche "OK" aus DataForm verwendet wird.
            // Wenn die Gültigkeit des Formulars nicht sichergestellt ist, wird sonst eine Ausnahme aufgerufen, die den Vorgang ausführt, falls die Entität ungültig ist.
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
        /// Vervollständigungshandler für den Registrierungsvorgang. 
        /// Wenn ein Fehler aufgetreten ist, wird dem Benutzer ein <see cref="ErrorWindow"/> angezeigt.
        /// Andernfalls wird ein Anmeldevorgang ausgeführt, der den soeben registrierten Benutzer automatisch anmeldet.
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
        /// Vervollständigungshandler für den Anmeldevorgang, der nach einer erfolgreichen Registrierung und einem Anmeldeversuch auftritt.
        /// Dadurch wird das Fenster geschlossen. Wenn der Vorgang fehlschlägt, wird die Fehlermeldung in einem <see cref="ErrorWindow"/> angezeigt.
        /// </summary>
        /// <param name="loginOperation">Der <see cref="LoginOperation"/>, der abgeschlossen wurde.</param>
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
                    // Der Vorgang war erfolgreich, die tatsächliche Anmeldung dagegen nicht.
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// Wechselt zum Anmeldefenster.
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// Falls ein Registrierungs- oder Anmeldevorgang ausgeführt wird und dieser abgebrochen werden kann, brechen Sie diesen ab.
        /// Schließen Sie andernfalls das Fenster.
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
        /// Ordnet der Schaltfläche "Abbrechen" die Esc-Taste und der Schaltfläche "OK" die EINGABETASTE zu.
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
        /// Legt den Fokus auf das Textfeld "Benutzername" fest.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
