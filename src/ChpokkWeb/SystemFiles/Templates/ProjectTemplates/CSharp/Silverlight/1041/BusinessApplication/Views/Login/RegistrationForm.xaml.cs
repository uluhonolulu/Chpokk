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
    /// <see cref="RegistrationData"/> を表し、登録プロセスを実行するフォームです。
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// 新しい <see cref="RegistrationForm"/> インスタンスを作成します。
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // 簡単にバインディングできるように、このコントロールの DataContext を Registration インスタンスに設定します。
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// 現在の <see cref="RegistrationForm"/> の親ウィンドウを設定します。
        /// </summary>
        /// <param name="window">親として使用するウィンドウです。</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// Password アクセサーおよび PasswordConfirmation アクセサーを生成されたフィールドとして接続します。
        /// Question フィールドを秘密の質問でいっぱいの ComboBox にバインドし、UserName TextBox の LostFocus イベントを処理します。
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // すべてのフィールドを追加モードにします
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
        /// UserName TextBox がフォーカスを失ったときのコールバックです。
        /// ロジックが処理されるように登録データを呼び出し、場合によって FriendlyName フィールドを設定します。
        /// </summary>
        /// <param name="sender">イベントの送信者です。</param>
        /// <param name="e">イベントの引数です。</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// <see cref="SecurityQuestions" /> で定義されているリソース文字列の一覧を返します。
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // リフレクションを使用して、ローカライズされた秘密の質問をすべて取得します
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// 新しい登録を送信します。
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // DataForm から標準の [OK] ボタンを使用していないため、強制的に検証する必要があります。
            // フォームが有効であることを確認しないと、エンティティが無効な場合、操作を呼び出す例外が発生します。
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
        /// 登録操作の完了ハンドラーです。
        /// エラーが発生した場合、<see cref="ErrorWindow"/> がユーザーに表示されます。
        /// それ以外の場合は、登録したユーザーが自動的にログインするログイン操作が呼び出されます。
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
        /// 登録およびログインに成功した後に行われるログイン操作の完了ハンドラーです。
        /// これにより、ウィンドウが閉じられます。操作に失敗した場合は、<see cref="ErrorWindow"/> にエラー メッセージが表示されます。
        /// </summary>
        /// <param name="loginOperation">完了した <see cref="LoginOperation"/> です。</param>
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
                    // 操作は成功しましたが、実際のログインは行われませんでした
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// ログイン ウィンドウに切り替えます。
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// 登録またはログイン操作の実行中で、キャンセルできる場合は、キャンセルします。
        /// それ以外の場合は、ウィンドウを閉じます。
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
        /// Esc キーを [キャンセル] ボタン、Enter キーを [OK] ボタンにマップします。
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
        /// フォーカスをユーザー名テキスト ボックスに設定します。
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
