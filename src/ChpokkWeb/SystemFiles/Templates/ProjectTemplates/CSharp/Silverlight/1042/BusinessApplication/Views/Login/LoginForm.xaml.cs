namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// 로그인 필드를 표시하고 로그인 프로세스를 처리하는 폼입니다.
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// 새 <see cref="LoginForm"/> 인스턴스를 만듭니다.
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // 쉽게 바인딩할 수 있도록 이 컨트롤의 DataContext를 LoginInfo 인스턴스로 설정합니다.
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// 현재 <see cref="LoginForm"/>에 대한 부모 창을 설정합니다.
        /// </summary>
        /// <param name="window">부모로 사용할 창입니다.</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// PasswordAccessor를 제공하도록 <see cref="DataForm.AutoGeneratingField"/>를 처리합니다.
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
        /// <see cref="LoginOperation"/>을 서버로 전송합니다.
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // DataForm에서 표준 확인 단추를 사용하지 않기 때문에 유효성 검사를 강제로 수행해야 합니다.
            // 폼이 올바른지 확인하지 않으면 엔터티가 잘못된 경우 작업을 호출하는 동안 예외가 발생합니다.
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// <see cref="LoginOperation"/>에 대한 완료 처리기입니다.
        /// 작업이 성공하면 창을 닫습니다.
        /// 오류가 있는 경우 <see cref="ErrorWindow"/>가 표시되고 오류가 처리된 것으로 표시됩니다.
        /// 취소하지 않았지만 로그인이 실패한 경우 자격 증명이 올바르지 않아서 사용자에게 알리기 위한 유효성 검사 오류가 추가되기 때문입니다.
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
        /// 등록 폼으로 전환합니다.
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// 로그인 작업이 진행 중이고 취소할 수 있는 경우 로그인 작업을 취소합니다.
        /// 그렇지 않으면 창을 닫습니다.
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
        /// Esc 키를 취소 단추에 매핑하고 Enter 키를 확인 단추에 매핑합니다.
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
        /// 사용자 이름 입력란에 포커스를 설정합니다.
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
