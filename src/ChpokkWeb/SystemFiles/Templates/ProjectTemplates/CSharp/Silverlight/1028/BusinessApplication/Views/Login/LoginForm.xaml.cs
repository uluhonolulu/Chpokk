namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// 呈現登入欄位並處理登入程序的表單。
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// 建立新 <see cref="LoginForm"/> 執行個體。
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // 將此控制項的 DataContext 設定為 LoginInfo 執行個體以便於繫結。
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// 設定目前 <see cref="LoginForm"/> 的父視窗。
        /// </summary>
        /// <param name="window">要用來當做父代的視窗。</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// 處理 <see cref="DataForm.AutoGeneratingField"/> 來提供 PasswordAccessor。
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
        /// 送出 <see cref="LoginOperation"/> 到伺服器
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // 我們必須強制驗證，因為我們不是使用 DataForm 的標準 [確定] 按鈕。
            // 如果不確認表單是否有效，則萬一實體無效，便可能在叫用作業時發生例外狀況。
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// <see cref="LoginOperation"/> 的完成處理常式。
        /// 如果作業成功，便會關閉視窗。
        /// 如果有錯誤，便顯示 <see cref="ErrorWindow"/> 並將此錯誤標示為已處理。
        /// 如果並未取消，但登入失敗，那一定是因為認證不正確，所以會加入驗證錯誤來通知使用者。
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
        /// 切換到登入表單。
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// 如果登入作業正在進行而且可以取消，請取消。
        /// 否則即關閉視窗。
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
        /// 將 Esc 對應到取消按鈕、Enter 對應到確定按鈕。
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
        /// 將焦點設定到使用者名稱文字方塊。
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
