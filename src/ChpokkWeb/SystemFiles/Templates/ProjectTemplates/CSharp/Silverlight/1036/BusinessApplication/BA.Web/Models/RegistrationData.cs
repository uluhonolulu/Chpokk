namespace $safeprojectname$
{
    using System.ComponentModel.DataAnnotations;
    using $safeprojectname$.Resources;

    /// <summary>
    /// Classe contenant les valeurs et les règles de validation pour l'inscription de l'utilisateur.
    /// </summary>
    public sealed partial class RegistrationData
    {
        /// <summary>
        /// Obtient et définit le nom d'utilisateur.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 0, Name = "UserNameLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessageResourceName = "ValidationErrorInvalidUserName", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "ValidationErrorBadUserNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string UserName { get; set; }

        /// <summary>
        /// Obtient et définit l'adresse de messagerie.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 2, Name = "EmailLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
                           ErrorMessageResourceName = "ValidationErrorInvalidEmail", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Email { get; set; }

        /// <summary>
        /// Obtient et définit le nom convivial de l'utilisateur.
        /// </summary>
        [Display(Order = 1, Name = "FriendlyNameLabel", Description = "FriendlyNameDescription", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(255, MinimumLength = 0, ErrorMessageResourceName = "ValidationErrorBadFriendlyNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string FriendlyName { get; set; }

        /// <summary>
        /// Obtient et définit la question de sécurité.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 5, Name = "SecurityQuestionLabel", ResourceType = typeof(RegistrationDataResources))]
        public string Question { get; set; }

        /// <summary>
        /// Obtient et définit la réponse à la question de sécurité.
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 6, Name = "SecurityAnswerLabel", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(128, ErrorMessageResourceName = "ValidationErrorBadAnswerLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Answer { get; set; }
    }
}
