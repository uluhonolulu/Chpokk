namespace $safeprojectname$
{
    using System.ComponentModel.DataAnnotations;
    using $safeprojectname$.Resources;

    /// <summary>
    /// ユーザー登録の値および検証規則を含むクラスです。
    /// </summary>
    public sealed partial class RegistrationData
    {
        /// <summary>
        /// ユーザー名を取得および設定します。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 0, Name = "UserNameLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression("^[a-zA-Z0-9_]*$", ErrorMessageResourceName = "ValidationErrorInvalidUserName", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [StringLength(255, MinimumLength = 4, ErrorMessageResourceName = "ValidationErrorBadUserNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string UserName { get; set; }

        /// <summary>
        /// 電子メール アドレスを取得および設定します。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 2, Name = "EmailLabel", ResourceType = typeof(RegistrationDataResources))]
        [RegularExpression(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", 
                           ErrorMessageResourceName = "ValidationErrorInvalidEmail", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Email { get; set; }

        /// <summary>
        /// ユーザーのフレンドリ名を取得および設定します。
        /// </summary>
        [Display(Order = 1, Name = "FriendlyNameLabel", Description = "FriendlyNameDescription", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(255, MinimumLength = 0, ErrorMessageResourceName = "ValidationErrorBadFriendlyNameLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string FriendlyName { get; set; }

        /// <summary>
        /// 秘密の質問を取得および設定します。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 5, Name = "SecurityQuestionLabel", ResourceType = typeof(RegistrationDataResources))]
        public string Question { get; set; }

        /// <summary>
        /// 秘密の質問に対する答えを取得および設定します。
        /// </summary>
        [Required(ErrorMessageResourceName = "ValidationErrorRequiredField", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        [Display(Order = 6, Name = "SecurityAnswerLabel", ResourceType = typeof(RegistrationDataResources))]
        [StringLength(128, ErrorMessageResourceName = "ValidationErrorBadAnswerLength", ErrorMessageResourceType = typeof(ValidationErrorResources))]
        public string Answer { get; set; }
    }
}
