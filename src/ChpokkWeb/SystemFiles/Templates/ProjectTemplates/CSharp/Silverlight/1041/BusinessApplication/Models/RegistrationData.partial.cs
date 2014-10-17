namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// クライアント側のカスタム検証および <see cref="RegistrationData"/> へのデータ バインディングを指定するための拡張機能です。
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// パスワードを返す関数を取得または設定します。
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// パスワードを取得および設定します。
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

                // パスワードをプレーン テキストでメモリに保存することはできないため、パスワードをプライベート フィールドに保存しないでください。
                // 代わりに、指定された PasswordAccessor が値のバッキング ストアとして機能します。

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// パスワードの確認を返す関数を取得または設定します。
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// パスワードの確認文字列を取得および設定します。
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

                // パスワードをプレーン テキストでメモリに保存することはできないため、パスワードをプライベート フィールドに保存しないでください。
                // 代わりに、指定された PasswordAccessor が値のバッキング ストアとして機能します。

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// 現在の登録操作またはログイン操作を取得または設定します。
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
        /// ユーザーが現在登録されているか、またはログインしているかを示す値を取得します。
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
        /// 現在の操作が変更された場合のヘルパー メソッドです。
        /// 適切なプロパティの変更通知を送信するために使用されます。
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// パスワードとパスワードの確認が一致することを確認します。
        /// 一致しない場合は、検証エラーが追加されます。
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // いずれかのパスワードが入力されていない場合、フィールドが同じかどうかのテストは行われません。
            // Required 属性により、両方のフィールドに必ず値が入力されるようになります。
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // 値が異なる場合は、両方のメンバーが指定された検証エラーを追加します
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// UserName 値の入力後、ロジックを実行します。
        /// </summary>
        /// <param name="userName">入力されたユーザー名の値です。</param>
        /// <remarks>
        /// 値が完全に入力されたことをフォームが示すようにします。
        /// OnUserNameChanged メソッドを使用すると、ユーザーがフォームで値の入力を完了する前に途中で呼び出すことができます。
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // フレンドリ名が指定されていない場合は、新しいエンティティの UserName と一致するように FriendlyName が自動入力されます
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// このエンティティのデータで初期化された新しい <see cref="LoginParameters"/> を作成します (IsPersistent の既定値は false になります)。
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
