namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Formulario que presenta los campos de inicio de sesión y controla el proceso de inicio de sesión.
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// Crea una nueva instancia de <see cref="LoginForm"/>.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // Establezca el DataContext de este control en la instancia de LoginInfo para facilitar los enlaces.
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// Establece la ventana primaria del <see cref="LoginForm"/> actual.
        /// </summary>
        /// <param name="window">Ventana que se va a utilizar como primaria.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Controla el <see cref="DataForm.AutoGeneratingField"/> para proporcionar el PasswordAccessor.
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
        /// Envía la <see cref="LoginOperation"/> al servidor
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Es necesario forzar la validación, ya que no se está utilizando el botón Aceptar estándar del DataForm.
            // Si no se garantiza que el formulario sea válido, se obtiene una excepción que invoca la operación en caso de que la entidad no sea válida.
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// Controlador de finalización de una <see cref="LoginOperation"/>.
        /// Si la operación es correcta, cierra la ventana.
        /// Si tiene un error, muestra una <see cref="ErrorWindow"/> y marca el error como controlado.
        /// Si no se canceló pero se produjo un error de inicio de sesión, debe haber sido porque las credenciales eran incorrectas, así que se agrega un error de validación para notificar al usuario.
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
        /// Cambia al formulario de registro.
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// Si hay una operación de inicio de sesión en curso y se puede cancelar, hágalo.
        /// De lo contrario, cierre la ventana.
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
        /// Asigna Esc al botón Cancelar y Entrar al botón Aceptar.
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
        /// Establece el enfoque en el cuadro de texto de nombre de usuario.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
