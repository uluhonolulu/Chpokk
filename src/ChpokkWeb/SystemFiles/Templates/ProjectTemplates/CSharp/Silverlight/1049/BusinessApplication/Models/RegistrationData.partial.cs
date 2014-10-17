namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// Расширения для привязки и пользовательской проверки данных <see cref="RegistrationData"/> на стороне клиента.
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// Возвращает или задает функцию, возвращающую пароль.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Возвращает и задает пароль.
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

                // Не сохраняйте пароль в скрытых полях, так как он не должен храниться в памяти в виде обычного текста.
                // Вместо этого для сохранения значения используйте объект PasswordAccessor.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Возвращает или задает функцию, возвращающую подтверждение пароля.
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// Возвращает и задает строку подтверждения пароля.
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

                // Не сохраняйте пароль в скрытых полях, так как он не должен храниться в памяти в виде обычного текста.  
                // Вместо этого для сохранения значения используйте объект PasswordAccessor.

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// Возвращает или задает текущую операцию регистрации или входа.
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
        /// Возвращает значение, указывающее, производит ли пользователь в настоящее время регистрацию или вход.
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
        /// Вспомогательный метод для изменения текущей операции.
        /// Служит для вызова соответствующих уведомлений об изменении свойств.
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// Проверяет пароль на соответствие подтверждению пароля.
        /// Если нет совпадения, добавляется ошибка проверки.
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // Если пароль или подтверждение не введены, эти поля на равенство не проверяются.
            // Атрибут Required обеспечивает ввод значений в обоих полях.
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // Если значения различаются, добавляется ошибка проверки, связанная с обоими указанными элементами.
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// Выполняет логику после ввода значения UserName.
        /// </summary>
        /// <param name="userName">Введенное имя пользователя.</param>
        /// <remarks>
        /// Позволяет форме указывать на неполный ввод значения.
        /// Использование метода OnUserNameChanged может привести к преждевременному вызову, прежде чем пользователь завершит ввод значения в форме.
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // Автозаполнение FriendlyName для новых сущностей, соответствующего UserName, когда понятное пользователю имя не указано
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// Создает новый объект <see cref="LoginParameters"/> и инициализирует его данными этой сущности (свойство IsPersistent по умолчанию имеет значение false).
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
