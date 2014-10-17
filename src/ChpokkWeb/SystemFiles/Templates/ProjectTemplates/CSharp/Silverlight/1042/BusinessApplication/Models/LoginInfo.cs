namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// 이 내부 엔터티는 UI 컨트롤(유효성 검사 오류를 표시하는 레이블 및 DataForm)과 사용자가 입력한 로그온 자격 증명 간의 바인딩을 용이하게 하는 데 사용됩니다.
    /// </summary>
    public class LoginInfo : ComplexObject
    {
        private string userName;
        private bool rememberMe;
        private LoginOperation currentLoginOperation;

        /// <summary>
        /// 사용자 이름을 가져오거나 설정합니다.
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
        /// 암호를 반환하는 함수를 가져오거나 설정합니다.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// 암호를 가져오거나 설정합니다.
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

                // 암호를 메모리에 일반 텍스트로 저장해서는 안 되므로 암호를 전용 필드에 저장하지 마십시오.
                // 대신, 제공된 PasswordAccessor가 값에 대한 백업 저장소 역할을 합니다.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// 이후의 로그인을 위해 사용자의 인증 정보를 기록할지 여부를 나타내는 값을 가져오거나 설정합니다.
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
        /// 현재 로그인 작업을 가져오거나 설정합니다.
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
        /// 사용자가 현재 로그인해 있는지 여부를 나타내는 값을 가져옵니다.
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
        /// 사용자가 현재 로그인할 수 있는지 여부를 나타내는 값을 가져옵니다.
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
        /// 현재 로그인 작업이 변경되면 작업 관련 속성 변경 알림을 표시합니다.
        /// </summary>
        private void CurrentLoginOperationChanged()
        {
            this.RaisePropertyChanged("IsLoggingIn");
            this.RaisePropertyChanged("CanLogIn");
        }

        /// <summary>
        /// 이 엔터티에 저장된 데이터를 사용하여 새 <see cref="LoginParameters"/> 인스턴스를 만듭니다.
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, this.RememberMe, null);
        }
    }
}
