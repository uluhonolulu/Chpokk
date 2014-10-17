namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// Форма, которая содержит поля для входа и обрабатывает вход.
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="LoginForm"/>.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // Задайте свойству DataContext этого элемента управления экземпляр LoginInfo, чтобы упростить привязку.
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// Устанавливает родительское окно для текущего окна <see cref="LoginForm"/>.
        /// </summary>
        /// <param name="window">Окно, которое будет использоваться в качестве родительского.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Обрабатывает поле <see cref="DataForm.AutoGeneratingField"/>, чтобы получить значение PasswordAccessor.
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
        /// Отправляет <see cref="LoginOperation"/> на сервер
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // Необходима принудительная проверка, поскольку стандартная кнопка "ОК" для DataForm не используется.
            // Без проверки правильности формы в случае, если сущность содержит неверные данные, возникнет исключение (exception) при вызове операции.
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// Обработчик завершения для операции <see cref="LoginOperation"/>.
        /// Если операция завершилась успешно, окно закрывается.
        /// При обнаружении ошибки отображается окно <see cref="ErrorWindow"/>, а ошибка помечается как обработанная.
        /// Если операция не была отменена, но вход завершился с ошибкой, вероятно, что учетные данные были введены неверно, поэтому уведомление об ошибке проверки подлинности направляется пользователю.
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
        /// Переключение на форму регистрации.
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// Если выполняется операция входа, которую можно отменить, она отменяется.
        /// В противном случае окно закрывается.
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
        /// Сопоставляет клавишу Esc с кнопкой "Отмена", а клавишу ВВОД с кнопкой "ОК".
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
        /// Устанавливает фокус в текстовом поле имени пользователя.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
