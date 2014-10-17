namespace $safeprojectname$
{
    using System.ComponentModel.DataAnnotations;
    using $safeprojectname$.Resources;

    /// <summary>
    /// 包含用于用户注册的值和验证规则的类。
    /// </summary>
    public sealed partial class RegistrationData
    {
        /// <summary>
        /// 获取和设置用户名。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 0, Name = "UserNameLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessageResourceName = "ValidationErrorInvalidUserName", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "ValidationErrorBadUserNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string UserName { get; set; }

        /// <summary>
        /// 获取和设置电子邮件地址。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 2, Name = "EmailLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
                           ErrorMessageResourceName = "ValidationErrorInvalidEmail", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Email { get; set; }

        /// <summary>
        /// 获取和设置用户的友好名称。
        /// </summary>
        [Display(Order = 1, Name = "FriendlyNameLabel", Description = "FriendlyNameDescription", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(255, MinimumLength = 0, ErrorMessageResourceName = "ValidationErrorBadFriendlyNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string FriendlyName { get; set; }

        /// <summary>
        /// 获取和设置安全提示问题。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 5, Name = "SecurityQuestionLabel", ResourceType = typeof(RegistrationDataResources))]
        public string Question { get; set; }

        /// <summary>
        /// 获取和设置安全提示问题的答案。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 6, Name = "SecurityAnswerLabel", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(128, ErrorMessageResourceName = "ValidationErrorBadAnswerLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Answer { get; set; }
    }
}
