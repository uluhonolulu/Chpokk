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
    /// Форма, которая отображает <see cref="RegistrationData"/> и выполняет процедуру регистрации.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// Создает новый экземпляр класса <see cref="RegistrationForm"/>.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // Задает свойству DataContext этого элемента управления экземпляр Registration, чтобы упростить привязку.
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// Устанавливает родительское окно для текущего окна <see cref="RegistrationForm"/>.
        /// </summary>
        /// <param name="window">Окно, которое будет использоваться в качестве родительского.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Связывает методы доступа к полям Password и PasswordConfirmation при создании этих полей.
        /// Кроме того, связывает поле Question с элементом ComboBox, содержащим список секретных вопросов, и обрабатывает событие LostFocus для элемента управления TextBox с именем UserName.
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // Переводит все поля в режим добавления
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
        /// Обратный вызов. Вызывается, когда элемент управления TextBox с именем UserName теряет фокус.
        /// Вызывается при регистрации данных, давая возможность обработать логику и, возможно, установить поле FriendlyName.
        /// </summary>
        /// <param name="sender">Отправитель события.</param>
        /// <param name="e">Аргументы события.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// Возвращает список строк ресурсов, определенных в коллекции <see cref="SecurityQuestions" />.
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // С помощью отражения (reflection) получить все локализованные секретные вопросы
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// Отправить данные для новой регистрации.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // Необходима принудительная проверка, поскольку стандартная кнопка "ОК" для DataForm не используется.
            // Без проверки правильности формы в случае, если сущность содержит неверные данные, можно получить исключение (exception) при вызове операции.
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
        /// Обработчик завершения для операции регистрации. 
        /// При обнаружении ошибки пользователю выводится окно <see cref="ErrorWindow"/>.
        /// В противном случае инициируется операция входа, которая автоматически производит вход для только что зарегистрированного пользователя.
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
        /// Обработчик завершения для операции входа возникает после успешной регистрации и попытки входа.
        /// Закрывает окно. Если операция завершилась ошибкой, сообщение будет отображено в окне <see cref="ErrorWindow"/>.
        /// </summary>
        /// <param name="loginOperation">Завершаемая операция <see cref="LoginOperation"/>.</param>
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
                    // Операция выполнена успешно, но вход не выполнен
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// Переключается в окно входа.
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// Если выполняется операция регистрации или входа, которую можно отменить, она отменяется.
        /// В противном случае окно закрывается.
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
        /// Сопоставляет клавишу Esc с кнопкой "Отмена", а клавишу ВВОД с кнопкой "ОК".
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
        /// Устанавливает фокус в текстовом поле имени пользователя.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
