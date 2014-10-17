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
    /// 呈現 <see cref="RegistrationData"/> 並執行註冊程序的表單。
    /// </summary>
    public partial class RegistrationForm : StackPanel
    {
        private LoginRegistrationWindow parentWindow;
        private RegistrationData registrationData = new RegistrationData();
        private UserRegistrationContext userRegistrationContext = new UserRegistrationContext();
        private TextBox userNameTextBox;

        /// <summary>
        /// 建立新 <see cref="RegistrationForm"/> 執行個體。
        /// </summary>
        public RegistrationForm()
        {
            InitializeComponent();

            // 將此控制項的 DataContext 設定為 Registration 執行個體以便於繫結。
            this.DataContext = this.registrationData;
        }

        /// <summary>
        /// 設定目前 <see cref="RegistrationForm"/> 的父視窗。
        /// </summary>
        /// <param name="window">要用來當做父代的視窗。</param>
        public void SetParentWindow(LoginRegistrationWindow window)
        {
            this.parentWindow = window;
        }

        /// <summary>
        /// 於欄位產生時串聯起 Password 和 PasswordConfirmation 存取子。
        /// 同時將 [Question] 欄位繫結到充滿安全性問題的 ComboBox，並且處理 UserName TextBox 的 LostFocus 事件。
        /// </summary>
        private void RegisterForm_AutoGeneratingField(object dataForm, DataFormAutoGeneratingFieldEventArgs e)
        {
            // 將所有欄位置於加入模式
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
        /// UserName TextBox 失去焦點時的回呼。
        /// 呼叫註冊資料以便對邏輯進行處理，可能需設定 [FriendlyName] 欄位。
        /// </summary>
        /// <param name="sender">事件發送者。</param>
        /// <param name="e">事件引數。</param>
        private void UserNameLostFocus(object sender, RoutedEventArgs e)
        {
            this.registrationData.UserNameEntered(((TextBox)sender).Text);
        }

        /// <summary>
        /// 傳回 <see cref="SecurityQuestions" /> 中所定義資源字串的清單。
        /// </summary>
        private static IEnumerable<string> GetSecurityQuestions()
        {
            // 使用反射來取得所有當地語系化的安全性問題
            return from propertyInfo in typeof(SecurityQuestions).GetProperties()
                   where propertyInfo.PropertyType.Equals(typeof(string))
                   select (string)propertyInfo.GetValue(null, null);
        }

        /// <summary>
        /// 送出新註冊。
        /// </summary>
        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            // 我們必須強制驗證，因為我們不是使用 DataForm 的標準 [確定] 按鈕。
            // 如果不確認表單是否有效，則萬一實體無效，便可能在叫用作業時發生例外狀況。
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
        /// 註冊作業的完成處理常式。
        /// 如果有錯誤，便向使用者顯示 <see cref="ErrorWindow"/>。
        /// 否則，它將觸發會自動登入剛註冊之使用者的登入作業。
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
        /// 發生於註冊及登入嘗試成功後的登入作業的完成處理常式。
        /// 這樣將會關閉視窗。如果作業失敗，<see cref="ErrorWindow"/> 將會顯示錯誤訊息。
        /// </summary>
        /// <param name="loginOperation">已完成的 <see cref="LoginOperation"/>。</param>
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
                    // 作業成功，但實際登入失敗
                    ErrorWindow.CreateNew(string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword));
                }
            }
        }

        /// <summary>
        /// 切換到登入視窗。
        /// </summary>
        private void BackToLogin_Click(object sender, RoutedEventArgs e)
        {
            this.parentWindow.NavigateToLogin();
        }

        /// <summary>
        /// 如果註冊或登入作業正在進行而且可以取消，便將它取消。
        /// 否則即關閉視窗。
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
        /// 將 Esc 對應到取消按鈕、Enter 對應到確定按鈕。
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
        /// 將焦點設定到使用者名稱文字方塊。
        /// </summary>
        public void SetInitialFocus()
        {
            this.userNameTextBox.Focus();
        }
    }
}
