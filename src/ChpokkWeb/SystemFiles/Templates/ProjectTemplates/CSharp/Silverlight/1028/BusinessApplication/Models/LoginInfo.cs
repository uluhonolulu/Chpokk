namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// 這個內部實體用於簡化 UI 控制項 (DataForm 和顯示驗證錯誤的標籤) 以及使用者輸入之登入認證間的繫結。
    /// </summary>
    public class LoginInfo : ComplexObject
    {
        private string userName;
        private bool rememberMe;
        private LoginOperation currentLoginOperation;

        /// <summary>
        /// 取得和設定使用者名稱。
        /// </summary>
        [Display(Name = "UserNameLabel", ResourceType = typeof(RegistrationDataResources))]
        [Required]
        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                if (this.userName != value)
                {
                    this.ValidateProperty("UserName", value);
                    this.userName = value;
                    this.RaisePropertyChanged("UserName");
                }
            }
        }

        /// <summary>
        /// 取得或設定傳回密碼的函式。
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// 取得和設定密碼。
        /// </summary>
        [Display(Name = "PasswordLabel", ResourceType = typeof(RegistrationDataResources))]
        [Required]
        public string Password
        {
            get
            {
                return (this.PasswordAccessor == null) ? string.Empty : this.PasswordAccessor();
            }
            set
            {
                this.ValidateProperty("Password", value);

                // 不要將密碼儲存在私用欄位，因為密碼不能以純文字儲存在記憶體中。
                // 相反的，提供的 PasswordAccessor 用於做為值的備份存放區。

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// 取得和設定表示是否要記錄使用者驗證資訊供爾後登入使用的值。
        /// </summary>
        [Display(Name = "RememberMeLabel", ResourceType = typeof(ApplicationStrings))]
        public bool RememberMe
        {
            get
            {
                return this.rememberMe;
            }

            set
            {
                if (this.rememberMe != value)
                {
                    this.ValidateProperty("RememberMe", value);
                    this.rememberMe = value;
                    this.RaisePropertyChanged("RememberMe");
                }
            }
        }

        /// <summary>
        /// 取得或設定目前的登入作業。
        /// </summary>
        internal LoginOperation CurrentLoginOperation
        {
            get
            {
                return this.currentLoginOperation;
            }
            set
            {
                if (this.currentLoginOperation != value)
                {
                    if (this.currentLoginOperation != null)
                    {
                        this.currentLoginOperation.Completed -= (s, e) => this.CurrentLoginOperationChanged();
                    }

                    this.currentLoginOperation = value;

                    if (this.currentLoginOperation != null)
                    {
                        this.currentLoginOperation.Completed += (s, e) => this.CurrentLoginOperationChanged();
                    }

                    this.CurrentLoginOperationChanged();
                }
            }
        }

        /// <summary>
        /// 取得表示使用者目前是否正在登入的值。
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool IsLoggingIn
        {
            get
            {
                return this.CurrentLoginOperation != null && !this.CurrentLoginOperation.IsComplete;
            }
        }

        /// <summary>
        /// 取得表示使用者目前是否可以登入的值。
        /// </summary>
        [Display(AutoGenerateField = false)]
        public bool CanLogIn
        {
            get
            {
                return !this.IsLoggingIn;
            }
        }

        /// <summary>
        /// 於目前登入作業變更時引發作業相關屬性變更通知。
        /// </summary>
        private void CurrentLoginOperationChanged()
        {
            this.RaisePropertyChanged("IsLoggingIn");
            this.RaisePropertyChanged("CanLogIn");
        }

        /// <summary>
        /// 使用此實體中儲存的資料建立新 <see cref="LoginParameters"/> 執行個體。
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, this.RememberMe, null);
        }
    }
}
