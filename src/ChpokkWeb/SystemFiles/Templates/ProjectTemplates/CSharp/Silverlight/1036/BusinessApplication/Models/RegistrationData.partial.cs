namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ServiceModel.DomainServices.Client;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using $safeprojectname$.Web.Resources;

    /// <summary>
    /// Extensions permettant de fournir la validation personnalisée côté client et la liaison de données à <see cref="RegistrationData"/>.
    /// </summary>
    public partial class RegistrationData
    {
        private OperationBase currentOperation;

        /// <summary>
        /// Obtient ou définit une fonction qui retourne le mot de passe.
        /// </summary>
        internal Func<string> PasswordAccessor { get; set; }

        /// <summary>
        /// Obtient et définit le mot de passe.
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

                // Ne pas stocker le mot de passe dans un champ privé car il ne doit pas être stocké en mémoire en texte brut.
                // À la place, le PasswordAccessor fourni sert de magasin de stockage pour la valeur.

                this.RaisePropertyChanged("Password");
            }
        }

        /// <summary>
        /// Obtient ou définit une fonction qui retourne la confirmation du mot de passe.
        /// </summary>
        internal Func<string> PasswordConfirmationAccessor { get; set; }

        /// <summary>
        /// Obtient et définit la chaîne de confirmation du mot de passe.
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

                // Ne pas stocker le mot de passe dans un champ privé car il ne doit pas être stocké en mémoire en texte brut.  
                // À la place, le PasswordAccessor fourni sert de magasin de stockage pour la valeur.

                this.RaisePropertyChanged("PasswordConfirmation");
            }
        }

        /// <summary>
        /// Obtient ou définit l'opération d'inscription ou de connexion en cours.
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
        /// Obtient une valeur indiquant si l'utilisateur est actuellement en cours d'inscription ou de connexion.
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
        /// Méthode d'assistance quand l'opération en cours change.
        /// Utilisé pour déclencher les notifications de modification de propriété appropriées.
        /// </summary>
        private void CurrentOperationChanged()
        {
            this.RaisePropertyChanged("IsRegistering");
        }

        /// <summary>
        /// Vérifie que le mot de passe et la confirmation sont identiques.
        /// S'ils ne correspondent pas, une erreur de validation est ajoutée.
        /// </summary>
        private void CheckPasswordConfirmation()
        {
            // Si l'un des mots de passe n'a pas encore été entré, ne pas tester l'égalité entre les champs.
            // L'attribut Required garantit qu'une valeur a été entrée pour les deux champs.
            if (string.IsNullOrWhiteSpace(this.Password)
                || string.IsNullOrWhiteSpace(this.PasswordConfirmation))
            {
                return;
            }

            // Si les valeurs sont différentes, ajouter une erreur de validation aux deux membres spécifiés
            if (this.Password != this.PasswordConfirmation)
            {
                this.ValidationErrors.Add(new ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, new string[] { "PasswordConfirmation", "Password" }));
            }
        }

        /// <summary>
        /// Exécuter la logique une fois la valeur UserName entrée.
        /// </summary>
        /// <param name="userName">Valeur de nom d'utilisateur qui a été entrée.</param>
        /// <remarks>
        /// Permettre au formulaire d'indiquer quand la valeur a été complètement entrée.
        /// L'utilisation de la méthode OnUserNameChanged peut entraîner un appel prématuré avant que l'utilisateur ait terminé d'entrer la valeur dans le formulaire.
        /// </remarks>
        internal void UserNameEntered(string userName)
        {
            // FriendlyName à remplissage automatique pour faire correspondre UserName avec de nouvelles entrées lorsqu'aucun nom convivial n'est spécifié
            if (string.IsNullOrWhiteSpace(this.FriendlyName))
            {
                this.FriendlyName = userName;
            }
        }

        /// <summary>
        /// Crée un nouveau <see cref="LoginParameters"/> initialisé avec les données de cette entité (la valeur par défaut d'IsPersistent est false).
        /// </summary>
        public LoginParameters ToLoginParameters()
        {
            return new LoginParameters(this.UserName, this.Password, false, null);
        }
    }
}
