namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// 用以表示登录字段并处理登录过程的窗体。
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// 创建新 <see cref="LoginForm"/> 实例。
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // 将此控件的 DataContext 设置为 LoginInfo 实例，以便可以轻松地绑定。
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// 设置当前 <see cref="LoginForm"/> 的父窗口。
        /// </summary>
        /// <param name="window">要用作父级的窗口。</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// 处理 <see cref="DataForm.AutoGeneratingField"/> 以提供 PasswordAccessor。
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
        /// 将 <see cref="LoginOperation"/> 提交给服务器
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // 由于未使用 DataForm 中的标准“确定”按钮，因此需要强制进行验证。
            // 如果未确保窗体有效，则在实体无效时调用该操作会导致异常。
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// <see cref="LoginOperation"/> 的完成处理程序。
        /// 如果操作成功，则关闭窗口。
        /// 如果发生错误，则显示 <see cref="ErrorWindow"/> 并将错误标记为已处理。
        /// 如果未取消操作但是登录失败，则一定是因为凭据不正确，因此添加验证错误以通知用户。
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
        /// 切换为注册窗体。
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// 如果登录操作正在进行并且可以取消，则取消该操作。
        /// 否则，关闭窗口。
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
        /// 将 Esc 映射到取消按钮，将 Enter 映射到确定按钮。
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
        /// 将焦点设置到用户名文本框。
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
