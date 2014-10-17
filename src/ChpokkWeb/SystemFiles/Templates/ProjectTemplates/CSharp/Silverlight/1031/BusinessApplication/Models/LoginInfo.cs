namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// Diese interne Entität wird verwendet, um die Bindung zwischen den Steuerelementen der Benutzeroberfläche (DataForm und die Bezeichnung, die einen Validierungsfehler anzeigt) und den vom Benutzer eingegebenen Anmeldeinformationen zu lösen.
    /// </summary>
    public class LoginInfo : ComplexObject
    {
        private string userName;
        private bool rememberMe;
        private LoginOperation currentLoginOperation;

        /// <summary>
        /// Ruft den Benutzernamen ab und legt diesen fest.
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
        /// Ruft eine Funktion ab, die das Kennwort zurückgibt, bzw. legt diese fest.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Ruft das Kennwort ab bzw. legt dieses fest.
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

                // Das Kennwort sollte nicht in einem privaten Feld gespeichert werden, ebenso wie es im Speicher nicht in Klartext gespeichert werden sollte.
                // Stattdessen dient der angegebene PasswordAccessor als Sicherungsspeicher für den Wert.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Ruft den Wert ab, der angibt, ob die Anmeldeinformationen des Benutzers für zukünftige Anmeldungen gespeichert werden sollen, bzw. legt diesen fest.
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
        /// Ruft den derzeitigen Anmeldevorgang ab bzw. legt diesen fest.
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
        /// Ruft einen Wert ab, der angibt, ob der Benutzer gerade angemeldet wird.
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
        /// Ruft einen Wert ab, der angibt, ob der Benutzer sich gerade anmelden kann.
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
        /// Löst bei einer Änderung des aktuellen Anmeldevorgangs vorgangsbezogene Benachrichtigungen zur Änderung der Eigenschaft aus
        /// </summary>
        private void CurrentLoginOperationChanged()
        {
            this.RaisePropertyChanged("IsLoggingIn");
            this.RaisePropertyChanged("CanLogIn");
        }

        /// <summary>
        /// Erstellt mithilfe der in dieser Entität gespeicherten Daten eine neue Instanz von <see cref="LoginParameters"/>.
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, this.RememberMe, null);
        }
    }
}
