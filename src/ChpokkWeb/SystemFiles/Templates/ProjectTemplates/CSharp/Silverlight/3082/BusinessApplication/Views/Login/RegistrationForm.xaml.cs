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
    /// Formulario que presenta los <see cref="RegistrationData"/> y realiza el proceso de registro.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// Crea una nueva instancia de <see cref="RegistrationForm"/>.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // Establezca el DataContext de este control en la instancia de Registration para facilitar los enlaces.
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// Establece la ventana primaria del <see cref="RegistrationForm"/> actual.
        /// </summary>
        /// <param name="window">Ventana que se va a utilizar como primaria.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Aplique los descriptores de acceso Password y PasswordConfirmation a medida que los campos se generan.
        /// Enlace además el campo Question a un ComboBox lleno de preguntas de seguridad y controle el evento LostFocus del TextBox UserName.
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // Establecer todos los campos en modo de adición
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
        /// Devolución de llamada para cuando el TextBox UserName pierde el enfoque.
        /// Llamada a los datos de registro para permitir que se procese la lógica, posiblemente estableciendo el campo FriendlyName.
        /// </summary>
        /// <param name="sender">Remitente del evento.</param>
        /// <param name="e">Argumentos del evento.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// Devuelve una lista de las cadenas de recursos definidas en las <see cref="SecurityQuestions" />.
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // Utilizar reflection para obtener todas las preguntas de seguridad localizadas
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// Envíe el nuevo registro.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Es necesario forzar la validación, ya que no se está utilizando el botón Aceptar estándar del DataForm.
            // Si no se garantiza que el formulario sea válido, se obtiene una excepción que invoca la operación en caso de que la entidad no sea válida.
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
        /// Controlador de finalización de la operación de registro. 
        /// Si se produjo un error, se muestra una <see cref="ErrorWindow"/> al usuario.
        /// De lo contrario, desencadena una operación de inicio de sesión que automáticamente iniciará la sesión del usuario que se acaba de registrar.
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
        /// Controlador de finalización de la operación de inicio de sesión que se produce tras un intento de registro e inicio de sesión correcto.
        /// Esto cerrará la ventana. Si se produce un error en la operación, una <see cref="ErrorWindow"/> mostrará el mensaje de error.
        /// </summary>
        /// <param name="loginOperation"><see cref="LoginOperation"/> que se ha completado.</param>
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
                    // La operación fue correcta, pero el inicio de sesión real no
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// Cambia a la ventana de inicio de sesión.
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// Si hay una operación de registro o inicio de sesión en curso y se puede cancelar, hágalo.
        /// De lo contrario, cierre la ventana.
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
        /// Asigna Esc al botón Cancelar y Entrar al botón Aceptar.
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
        /// Establece el enfoque en el cuadro de texto de nombre de usuario.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
