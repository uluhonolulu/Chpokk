namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// <see cref="RegistrationData"/>에 클라이언트 쪽 사용자 지정 유효성 검사 및 데이터 바인딩을 제공하는 확장입니다.
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// 암호를 반환하는 함수를 가져오거나 설정합니다.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// 암호를 가져오거나 설정합니다.
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

                // 암호를 메모리에 일반 텍스트로 저장해서는 안 되므로 암호를 전용 필드에 저장하지 마십시오.
                // 대신, 제공된 PasswordAccessor가 값에 대한 백업 저장소 역할을 합니다.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// 암호 확인을 반환하는 함수를 가져오거나 설정합니다.
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// 암호 확인 문자열을 가져오거나 설정합니다.
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

                // 암호를 메모리에 일반 텍스트로 저장해서는 안 되므로 암호를 전용 필드에 저장하지 마십시오.  
                // 대신, 제공된 PasswordAccessor가 값에 대한 백업 저장소 역할을 합니다.

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// 현재 등록 또는 로그인 작업을 가져오거나 설정합니다.
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
        /// 사용자가 현재 등록되어 있거나 로그인해 있는지 여부를 나타내는 값을 가져옵니다.
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
        /// 현재 작업이 변경된 경우에 대한 도우미 메서드입니다.
        /// 적절한 속성 변경 알림을 표시하는 데 사용됩니다.
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// 암호와 암호 확인이 일치하는지 확인합니다.
        /// 일치하지 않는 경우 유효성 검사 오류가 추가됩니다.
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // 암호 또는 암호 확인을 입력하지 않은 경우 두 필드가 일치하는지 테스트하지 않습니다.
            // Required 특성은 두 필드 모두에 값이 입력되었는지 확인합니다.
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // 값이 다른 경우 두 멤버를 지정하여 유효성 검사 오류를 추가합니다.
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// UserName 값이 입력된 이후에 논리를 수행합니다.
        /// </summary>
        /// <param name="userName">입력된 사용자 이름 값입니다.</param>
        /// <remarks>
        /// 폼을 사용하여 값이 입력된 시간을 나타낼 수 있습니다.
        /// OnUserNameChanged 메서드를 사용하면 사용자가 폼에 값을 입력하기 전에 호출이 발생할 수 있습니다.
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // 표시 이름을 지정하지 않은 경우 새 엔터티에 대한 UserName과 일치하도록 FriendlyName을 자동으로 채웁니다.
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// 이 엔터티의 데이터로 초기화되는 새 <see cref="LoginParameters"/>를 만듭니다. IsPersistent의 기본값은 false입니다.
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
