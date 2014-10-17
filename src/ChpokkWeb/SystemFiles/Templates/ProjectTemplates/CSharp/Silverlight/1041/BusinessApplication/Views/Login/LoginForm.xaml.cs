namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;

    /// <summary>
    /// ログイン フィールドを表し、ログイン プロセスを処理するフォームです。
    /// </summary>
    public partial class LoginForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private LoginInfo loginInfo = new LoginInfo();
        private TextBox userNameTextBox;

        /// <summary>
        /// 新しい <see cref="LoginForm"/> インスタンスを作成します。
        /// </summary>
        public LoginForm()
        {
            InitializeComponent();

            // 簡単にバインディングできるように、このコントロールの DataContext を LoginInfo インスタンスに設定します。
            this.DataContext = this.loginInfo;
        }

        /// <summary>
        /// 現在の <see cref="LoginForm"/> の親ウィンドウを設定します。
        /// </summary>
        /// <param name="window">親として使用するウィンドウです。</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// <see cref="DataForm.AutoGeneratingField"/> を処理して PasswordAccessor を指定します。
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
        /// <see cref="LoginOperation"/> をサーバーに送信します
        /// </summary>
        private void LoginButton_Click(object sender, EventArgs e)
        {
            // DataForm から標準の [OK] ボタンを使用していないため、強制的に検証する必要があります。
            // フォームが有効であることを確認しないと、エンティティが無効な場合、操作を呼び出す例外が発生します。
            if (this.loginForm.ValidateItem())
            {
                this.loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(this.loginInfo.ToLoginParameters(), this.LoginOperation_Completed, null);
                this.parentWindow.AddPendingOperation(this.loginInfo.CurrentLoginOperation);
            }
        }

        /// <summary>
        /// <see cref="LoginOperation"/> の完了ハンドラーです。
        /// 操作が成功した場合、ウィンドウを閉じます。
        /// エラーがある場合、<see cref="ErrorWindow"/> を表示し、エラーを処理済みとしてマークします。
        /// キャンセルされずにログインに失敗した場合、資格情報が間違っており、ユーザーに通知するために検証エラーが追加されたことが原因です。
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
        /// 登録フォームに切り替えます。
        /// </summary>
        private void RegisterNow_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToRegistration();
        }

        /// <summary>
        /// ログイン操作の実行中で、キャンセルできる場合は、キャンセルします。
        /// それ以外の場合は、ウィンドウを閉じます。
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
        /// Esc キーを [キャンセル] ボタン、Enter キーを [OK] ボタンにマップします。
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
        /// フォーカスをユーザー名テキスト ボックスに設定します。
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
