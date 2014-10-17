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
    /// <see cref="RegistrationData"/>를 표시하고 등록 프로세스를 수행하는 폼입니다.
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// 새 <see cref="RegistrationForm"/> 인스턴스를 만듭니다.
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // 쉽게 바인딩할 수 있도록 이 컨트롤의 DataContext를 Registration 인스턴스로 설정합니다.
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// 현재 <see cref="RegistrationForm"/>에 대한 부모 창을 설정합니다.
        /// </summary>
        /// <param name="window">부모로 사용할 창입니다.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Password 및 PasswordConfirmation 접근자를 생성되는 필드로 연결합니다.
        /// Question 필드를 보안 질문 ComboBox에 바인딩하고 UserName TextBox에 대한 LostFocus 이벤트를 처리합니다.
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // 모든 필드를 추가 모드로 전환
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
        /// UserName TextBox가 포커스를 잃을 때 사용할 콜백입니다.
        /// FriendlyName 필드를 설정하여 논리를 처리할 수 있도록 등록 데이터를 호출합니다.
        /// </summary>
        /// <param name="sender">이벤트 전송자입니다.</param>
        /// <param name="e">이벤트 인수입니다.</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// <see cref="SecurityQuestions" />에 정의된 리소스 문자열 목록을 반환합니다.
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // 모든 지역화된 보안 질문을 가져오려면 리플렉션을 사용합니다.
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// 새 등록을 전송합니다.
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // DataForm에서 표준 확인 단추를 사용하지 않기 때문에 유효성 검사를 강제로 수행해야 합니다.
            // 폼이 올바른지 확인하지 않으면 엔터티가 잘못된 경우 작업을 호출하는 동안 예외가 발생합니다.
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
        /// 등록 작업에 대한 완료 처리기입니다. 
        /// 오류가 발생한 경우 <see cref="ErrorWindow"/>가 표시됩니다.
        /// 그렇지 않으면 방금 등록한 사용자를 자동으로 로그인시키는 로그인 작업이 트리거됩니다.
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
        /// 성공적인 등록 및 로그인 시도 후에 발생하는 로그인 작업에 대한 완료 처리기입니다.
        /// 창이 닫힙니다. 작업이 실패하면 <see cref="ErrorWindow"/>에 오류 메시지가 표시됩니다.
        /// </summary>
        /// <param name="loginOperation">완료된 <see cref="LoginOperation"/>입니다.</param>
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
                    // 작업이 성공했지만 실제 로그인은 실패했습니다.
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// 로그인 창으로 전환합니다.
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// 등록 또는 로그인 작업이 진행 중이고 취소할 수 있는 경우 작업을 취소합니다.
        /// 그렇지 않으면 창을 닫습니다.
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
        /// Esc 키를 취소 단추에 매핑하고 Enter 키를 확인 단추에 매핑합니다.
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
        /// 사용자 이름 입력란에 포커스를 설정합니다.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
