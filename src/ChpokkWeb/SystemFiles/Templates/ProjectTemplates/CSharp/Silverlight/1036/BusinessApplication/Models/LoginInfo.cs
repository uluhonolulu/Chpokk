namespace $safeprojectname$.LoginUI
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// L'entité interne est utilisée pour faciliter la liaison entre les contrôles d'interface utilisateur (DataForm et l'étiquette affichant une erreur de validation) et les informations d'identification de connexion entrées par l'utilisateur.
    /// </summary>
    public class LoginInfo : ComplexObject
    {
        private string userName;
        private bool rememberMe;
        private LoginOperation currentLoginOperation;

        /// <summary>
        /// Obtient et définit le nom d'utilisateur.
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
        /// Obtient ou définit une fonction qui retourne le mot de passe.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Obtient et définit le mot de passe.
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

                // Ne pas stocker le mot de passe dans un champ privé car il ne doit pas être stocké en mémoire en texte brut.
                // À la place, le PasswordAccessor fourni sert de magasin de stockage pour la valeur.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Obtient et définit la valeur indiquant si les informations d'authentification de l'utilisateur doivent être enregistrées pour des connexions ultérieures.
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
        /// Obtient ou définit l'opération de connexion en cours.
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
        /// Obtient une valeur indiquant si l'utilisateur est actuellement en cours de connexion.
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
        /// Obtient une valeur indiquant si l'utilisateur peut actuellement se connecter.
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
        /// Déclenche des notifications de modification de propriété liée à l'opération lorsque l'opération de connexion en cours change.
        /// </summary>
        private void CurrentLoginOperationChanged()
        {
            this.RaisePropertyChanged("IsLoggingIn");
            this.RaisePropertyChanged("CanLogIn");
        }

        /// <summary>
        /// Crée une nouvelle instance <see cref="LoginParameters"/> à l'aide des données stockées dans cette entité.
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, this.RememberMe, null);
        }
    }
}
