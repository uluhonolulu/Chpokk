namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// この内部エンティティは、UI コントロール (DataForm と検証エラーを表示するラベル) とユーザーが入力したログオン資格情報の間のバインディングを簡単にするために使用されます。
    /// </summary>
    public class LoginInfo : ComplexObject
    {
        private string userName;
        private bool rememberMe;
        private LoginOperation currentLoginOperation;

        /// <summary>
        /// ユーザー名を取得および設定します。
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
        /// パスワードを返す関数を取得または設定します。
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// パスワードを取得および設定します。
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

                // パスワードをプレーン テキストでメモリに保存することはできないため、パスワードをプライベート フィールドに保存しないでください。
                // 代わりに、指定された PasswordAccessor が値のバッキング ストアとして機能します。

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// 今後のログインのためにユーザーの認証情報を記録するかどうかを示す値を取得および設定します。
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
        /// 現在のログイン操作を取得または設定します。
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
        /// ユーザーが現在ログインしているかどうかを示す値を取得します。
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
        /// ユーザーが現在ログインできるかどうかを示す値を取得します。
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
        /// 現在のログイン操作が変更された場合、操作に関連するプロパティの変更通知を送信します。
        /// </summary>
        private void CurrentLoginOperationChanged()
        {
            this.RaisePropertyChanged("IsLoggingIn");
            this.RaisePropertyChanged("CanLogIn");
        }

        /// <summary>
        /// このエンティティに保存されたデータを使用して、新しい <see cref="LoginParameters"/> インスタンスを作成します。
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, this.RememberMe, null);
        }
    }
}
