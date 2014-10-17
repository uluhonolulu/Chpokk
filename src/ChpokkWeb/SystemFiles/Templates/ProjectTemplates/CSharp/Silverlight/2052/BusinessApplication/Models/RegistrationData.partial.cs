namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// 用于提供客户端自定义验证和对 <see cref="RegistrationData"/> 的数据绑定的扩展。
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// 获取或设置返回密码的函数。
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// 获取和设置密码。
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

                // 请不要将密码存储在私有字段中，因为不应以纯文本形式在内存中存储密码。
                // 而应将提供的 PasswordAccessor 用作该值的后备存储。

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// 获取或设置返回密码确认的函数。
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// 获取和设置密码确认字符串。
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

                // 请不要将密码存储在私有字段中，因为不应以纯文本形式在内存中存储密码。
                // 而应将提供的 PasswordAccessor 用作该值的后备存储。

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// 获取或设置当前注册或登录操作。
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
        /// 获取一个值，该值指示用户当前是否正在注册或登录。
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
        /// 在当前操作发生更改时要使用的帮助程序方法。
        /// 用于引发合适的属性更改通知。
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// 检查以确保密码与确认密码匹配。
        /// 如果两者不匹配，则添加验证错误。
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // 如果未输入两个密码中的任何一个，则不测试这两个字段的相等性。
            // Required 特性确保为这两个字段都输入了值。
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // 如果两个值不同，则添加一个验证错误，并同时指定这两个成员。
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// 在输入 UserName 值之后执行逻辑。
        /// </summary>
        /// <param name="userName">输入的用户名值。</param>
        /// <remarks>
        /// 允许窗体指示值在何时已输入完整。
        /// 使用 OnUserNameChanged 方法可能导致在用户在窗体中输入完整值之前过早进行调用。
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // 在没有指定友好名称时为新实体自动填充 FriendlyName 以匹配 UserName
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// 创建使用此实体的数据(IsPersistent 默认为 false)初始化的新 <see cref="LoginParameters"/>。
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
