namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// Extensiones para proporcionar validación personalizada de cliente y enlace de datos a <see cref="RegistrationData"/>.
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// Obtiene o establece una función que devuelve la contraseña.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Obtiene y establece la contraseña.
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

                // No almacene la contraseña en un campo privado: no se debe almacenar en memoria en texto sin formato.
                // En su lugar, el PasswordAccessor proporcionado sirve como almacén de copia de seguridad del valor.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Obtiene o establece una función que devuelve la confirmación de la contraseña.
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// Obtiene y establece la cadena de confirmación de la contraseña.
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

                // No almacene la contraseña en un field private: no se debe almacenar en memoria en texto sin formato.  
                // En su lugar, el PasswordAccessor proporcionado sirve como almacén de copia de seguridad del valor.

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// Obtiene o establece la operación de registro o inicio de sesión actual.
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
        /// Obtiene un valor que indica si el usuario se está registrando o iniciando sesión en ese momento.
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
        /// Método auxiliar para cuando cambia la operación actual.
        /// Se utiliza para generar notificaciones de cambio de propiedades adecuadas.
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// Comprueba para asegurarse de que la contraseña y la confirmación coinciden.
        /// Si no coinciden, se agrega un error de validación.
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // Si aún no se ha especificado alguna de las contraseñas, no pruebe la igualdad entre los campos.
            // El atributo Required garantizará que se ha especificado un valor para ambos campos.
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // Si los valores son distintos, agregar un error de validación con ambos miembros especificados
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// Realice la lógica una vez especificado el valor UserName.
        /// </summary>
        /// <param name="userName">Valor de nombre de usuario especificado.</param>
        /// <remarks>
        /// Permita que el formulario indique si el valor se ha especificado totalmente.
        /// El empleo del método OnUserNameChanged puede dar lugar a una llamada prematura antes de que el usuario haya terminado de especificar el valor en el formulario.
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // FriendlyName autorrellenado que coincide con el UserName de las nuevas entidades cuando no se ha especificado un nombre descriptivo
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// Crea un nuevo <see cref="LoginParameters"/> inicializado con los datos de esta entidad (IsPersistent se establecerá de forma predeterminada en false).
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
