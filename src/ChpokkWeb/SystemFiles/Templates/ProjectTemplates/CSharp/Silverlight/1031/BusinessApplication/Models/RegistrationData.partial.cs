namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// Erweiterungen für die Bereitstellung einer benutzerdefinierten Validierung auf Clientseite und Datenbindung an <see cref="RegistrationData"/>.
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// Ruft eine Funktion ab, die das Kennwort zurückgibt, bzw. legt diese fest.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Ruft das Kennwort ab bzw. legt dieses fest.
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

                // Das Kennwort sollte nicht in einem privaten Feld gespeichert werden, ebenso wie es im Speicher nicht in Klartext gespeichert werden sollte.
                // Stattdessen dient der angegebene PasswordAccessor als Sicherungsspeicher für den Wert.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Ruft eine Funktion ab, die die Kennwortbestätigung zurückgibt, bzw. legt diese fest.
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// Ruft die Zeichenfolge der Kennwortbestätigung ab, bzw. legt diese fest.
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

                // Das Kennwort sollte nicht in einem privaten Feld gespeichert werden, ebenso wie es im Speicher nicht in Klartext gespeichert werden sollte.  
                // Stattdessen dient der angegebene PasswordAccessor als Sicherungsspeicher für den Wert.

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// Ruft den derzeitigen Registrierungs- oder Anmeldevorgang ab bzw. legt diesen fest.
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
        /// Ruft einen Wert ab, der angibt, ob der Benutzer gerade registriert oder angemeldet wird.
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
        /// Hilfsmethode bei einer Änderung des derzeitigen Vorgangs.
        /// Wird verwendet, um entsprechende Benachrichtigungen für die Änderung der Eigenschaft auszulösen.
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// Überprüft, ob Passwort und Passwortbestätigung übereinstimmen.
        /// Ist dies nicht der Fall, wird ein Validierungsfehler angefügt.
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // Wenn eines der beiden Kennwörter noch nicht eingegeben wurde, diese beiden Felder nicht auf Übereinstimmung prüfen.
            // Das Required-Attribut stellt sicher, dass in beiden Feldern ein Wert eingegeben wurde.
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // Falls die Werte nicht übereinstimmen, wird ein Validierungsfehler angefügt, in dem beide Mitglieder angegeben sind
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// Logik durchführen, nachdem der Wert "UserName" eingegeben wurde
        /// </summary>
        /// <param name="userName">Der Wert "Benutzername", der eingegeben wurde.</param>
        /// <remarks>
        /// Zulassen, dass das Formular anzeigt, wann der Wert vollständig eingegeben wurde.
        /// Die Verwendung der Methode "OnUserNameChanged" kann zu einem verfrühten Aufruf führen, bevor der Benutzer die Eingabe des Wertes im Formular beendet hat.
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // AutoAusfüllen des FriendlyName, damit dieser mit UserName übereinstimmt, wenn bei neuen Entitäten kein Anzeigename angegeben ist
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// Erstellt neue <see cref="LoginParameters"/>, die mit den Daten dieser Entität initialisiert werden (IsPersistent erhält den Standardwert "false").
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
