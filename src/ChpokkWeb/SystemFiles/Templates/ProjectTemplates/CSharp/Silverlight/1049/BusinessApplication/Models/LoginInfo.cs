namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// Эта внутренняя сущность служит для упрощения связывания элементов пользовательского интерфейса (DataForm и метка отображают ошибку проверки) и учетных данных входа, введенных пользователем.
    /// </summary>
    public class LoginInfo : ComplexObject
    {
        private string userName;
        private bool rememberMe;
        private LoginOperation currentLoginOperation;

        /// <summary>
        /// Возвращает и задает имя пользователя.
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
        /// Возвращает или задает функцию, возвращающую пароль.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Возвращает и задает пароль.
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

                // Не сохраняйте пароль в скрытых полях, так как он не должен храниться в памяти в виде обычного текста.
                // Вместо этого для сохранения значения используйте объект PasswordAccessor.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Возвращает и задает значение, указывающее, нужно ли сохранить данные проверки подлинности пользователя для использования в будущем.
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
        /// Возвращает или задает текущую операцию входа.
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
        /// Возвращает значение, указывающее, производит ли пользователь в настоящее время вход.
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
        /// Возвращает значение, указывающее, произвел ли пользователь в настоящее время вход.
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
        /// Вызывает уведомления, связанные с изменением свойств операции входа в процессе ее изменения.
        /// </summary>
        private void CurrentLoginOperationChanged()
        {
            this.RaisePropertyChanged("IsLoggingIn");
            this.RaisePropertyChanged("CanLogIn");
        }

        /// <summary>
        /// Создает новый экземпляр класса <see cref="LoginParameters"/> на основе данных, хранящихся в этой сущности.
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, this.RememberMe, null);
        }
    }
}
