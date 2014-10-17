namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// 用來提供用戶端自訂驗證及資料繫結到 <see cref="RegistrationData"/> 的延伸模組。
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// 取得或設定傳回密碼的函式。
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// 取得和設定密碼。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 3, Name = "PasswordLabel", Description = "PasswordDescription", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName = "ValidationErrorBadPasswordStrength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [StringLength(50, MinimumLength = 7, ErrorMessageResourceName = "ValidationErrorBadPasswordLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Password
        {
            get
            {
                return (this.PasswordAccessor == null) ? string.Empty : this.PasswordAccessor();
            }

            set
            {
                this.ValidateProperty("Password", value);
                this.CheckPasswordConfirmation();

                // 不要將密碼儲存在私用欄位，因為密碼不能以純文字儲存在記憶體中。
                // 相反的，提供的 PasswordAccessor 用於做為值的備份存放區。

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// 取得或設定傳回密碼確認的函式。
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// 取得和設定密碼確認字串。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 4, Name = "PasswordConfirmationLabel", ResourceType = typeof(RegistrationDataResources))]
        public string PasswordConfirmation
        {
            get
            {
                return (this.PasswordConfirmationAccessor == null) ? string.Empty : this.PasswordConfirmationAccessor();
            }

            set
            {
                this.ValidateProperty("PasswordConfirmation", value);
                this.CheckPasswordConfirmation();

                // 不要將密碼儲存在私用欄位，因為密碼不能以純文字儲存在記憶體中。  
                // 相反的，提供的 PasswordAccessor 用於做為值的備份存放區。

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// 取得或設定目前的註冊或登入作業。
        /// </summary>
        internal OperationBase CurrentOperation
        {
            get
            {
                return this.currentOperation;
            }
            set
            {
                if (this.currentOperation != value)
                {
                    if (this.currentOperation != null)
                    {
                        this.currentOperation.Completed -= (s, e) => this.CurrentOperationChanged();
                    }

                    this.currentOperation = value;

                    if (this.currentOperation != null)
                    {
                        this.currentOperation.Completed += (s, e) => this.CurrentOperationChanged();
                    }

                    this.CurrentOperationChanged();
                }
            }
        }

        /// <summary>
        ///  取得表示使用者目前是否正在註冊或登入的值。
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool IsRegistering
        {
            get
            {
                return this.CurrentOperation != null && !this.CurrentOperation.IsComplete;
            }
        }

        /// <summary>
        /// 使用於目前作業變更時的 Helper 方法。
        /// 用來引發適當的屬性變更通知。
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// 檢查密碼與確認是否相符。
        /// 如果不相符，便加入驗證錯誤。
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // 如果有任何密碼尚未輸入，就不用測試欄位之間是否相等。
            // Required 屬性將確保兩個欄位都輸入了值。
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // 如果值不相同，就在指定的兩個成員都加入驗證錯誤
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// 於 UserName 值輸入之後執行邏輯。
        /// </summary>
        /// <param name="userName">輸入的使用者名稱值。</param>
        /// <remarks>
        /// 讓表單在值輸入完成時顯示指示。
        /// 使用 OnUserNameChanged 方法可能導致使用者完成表單中的輸入前過早呼叫。
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // 未指定易記名稱時自動填入 FriendlyName 來配合新實體的 UserName
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// 建立已用此實體的資料初始化的新 <see cref="LoginParameters"/> (IsPersistent 將預設為 false)。
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
