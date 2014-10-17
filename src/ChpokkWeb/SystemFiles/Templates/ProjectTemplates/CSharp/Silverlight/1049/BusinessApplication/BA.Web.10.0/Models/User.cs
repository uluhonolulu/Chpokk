namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// Класс содержит сведения о пользователе, прошедшем проверку подлинности.
    /// </summary>
    public partial class User : UserBase
    {
        //// ПРИМЕЧАНИЕ. Можно добавить свойства профиля для использования в приложении Silverlight.
        //// Чтобы включить профили, измените соответствующий раздел в файле web.config.
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// Возвращает и задает понятное имя пользователя.
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
