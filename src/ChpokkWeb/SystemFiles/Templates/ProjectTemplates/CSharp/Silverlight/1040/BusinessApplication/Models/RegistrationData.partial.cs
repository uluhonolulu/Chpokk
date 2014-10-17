namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// Estensioni per fornire la convalida personalizzata lato client e l'associazione dati a <see cref="RegistrationData"/>.
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// Ottiene o imposta una funzione che restituisce la password.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Ottiene e imposta la password.
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

                // Non archiviare la password in un campo privato poiché non deve essere memorizzata in testo normale.
                // L'oggetto PasswordAccessor fornito funge invece da archivio di backup per il valore.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Ottiene o imposta una funzione che restituisce la conferma della password.
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// Ottiene e imposta la stringa di conferma della password.
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

                // Non archiviare la password in un campo privato poiché non deve essere memorizzata in testo normale.  
                // L'oggetto PasswordAccessor fornito funge invece da archivio di backup per il valore.

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// Ottiene o imposta l'operazione di registrazione o di accesso corrente.
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
        /// Ottiene un valore che indica se è in corso la registrazione o l'accesso dell'utente.
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
        /// Metodo di supporto per la modifica dell'operazione corrente.
        /// Utilizzato per generare notifiche di modifica delle proprietà appropriate.
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// Verifica che la password e la conferma corrispondano.
        /// Se non corrispondono, viene aggiunto un errore di convalida.
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // Se non è stata ancora immessa una delle due password, non verificare la corrispondenza tra i campi.
            // L'attributo Required verificherà che sia stato immesso un valore in entrambi i campi.
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // Se i valori sono diversi, aggiungere un errore di convalida con entrambi i membri specificati
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// Eseguire la logica dopo avere immesso il valore UserName.
        /// </summary>
        /// <param name="userName">Valore nome utente immesso.</param>
        /// <remarks>
        /// Consentire al form di indicare quando il valore è stato immesso completamente.
        /// L'utilizzo del metodo OnUserNameChanged può far sì che la chiamata venga effettuata prima che l'utente abbia terminato di immettere il valore nel form.
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // Riempire automaticamente FriendlyName in modo da corrispondere a UserName per le nuove entità quando non viene specificato un nome descrittivo
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// Crea un nuovo oggetto <see cref="LoginParameters"/> inizializzato con i dati di questa entità (IsPersistent utilizzerà false come impostazione predefinita).
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
