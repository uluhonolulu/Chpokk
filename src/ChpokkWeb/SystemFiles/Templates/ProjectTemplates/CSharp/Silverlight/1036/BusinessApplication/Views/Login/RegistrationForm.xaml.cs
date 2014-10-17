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
    /// Formulaire qui présente les <see cref="RegistrationData"/> et exécute le processus d'inscription.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// Crée une nouvelle instance <see cref="RegistrationForm"/>.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // Affecter au DataContext de ce contrôle l'instance Registration pour permettre une liaison facile.
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// Définit la fenêtre parente pour le <see cref="RegistrationForm"/> actuel.
        /// </summary>
        /// <param name="window">Fenêtre à utiliser comme parent.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Connecter les accesseurs Password et PasswordConfirmation lors de la génération des champs.
        /// Également lier un champ Question à un ComboBox plein de questions de sécurité, et gérer l'événement LostFocus pour le TextBox UserName.
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // Placer tous les champs en mode d'ajout
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
        /// Rappel du moment où le TextBox UserName perd le focus.
        /// Appel des données d'inscription pour permettre le traitement de la logique, en définissant éventuellement le champ FriendlyName.
        /// </summary>
        /// <param name="sender">Émetteur de l'événement.</param>
        /// <param name="e">Arguments de l'événement.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// Retourne la liste des chaînes de ressource définies dans <see cref="SecurityQuestions" />.
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // Utiliser la réflexion pour obtenir toutes les questions de sécurité localisées
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// Soumettre une nouvelle inscription.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Nous devons forcer la validation car nous n'utilisons pas le bouton OK standard du DataForm.
            // Sans vérification de la validité du formulaire, nous obtenons une exception lors de l'appel de l'opération si l'entité n'est pas valide.
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
        /// Gestionnaire d'achèvement d'une opération d'inscription. 
        /// Si une erreur s'est produite, un <see cref="ErrorWindow"/> s'affiche à l'utilisateur.
        /// Sinon, cela déclenche une opération de connexion qui connecte automatiquement l'utilisateur qui vient d'être inscrit.
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
        /// Gestionnaire d'achèvement pour l'opération de connexion qui se produit après une tentative d'inscription et de connexion réussie.
        /// Cela ferme la fenêtre. Si l'opération échoue, un <see cref="ErrorWindow"/> affiche le message d'erreur.
        /// </summary>
        /// <param name="loginOperation"><see cref="LoginOperation"/> qui s'est terminé.</param>
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
                    // L'opération a réussi, mais pas la connexion réelle
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// Bascule vers la fenêtre de connexion.
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// Si une opération d'inscription ou de connexion est en cours et qu'elle peut être annulée, annulez-la.
        /// Sinon, fermer la fenêtre.
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
        /// Mappe Esc au bouton d'annulation et Entrée au bouton OK.
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
        /// Définit le focus sur la zone de texte du nom d'utilisateur.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
