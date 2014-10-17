namespace $safeprojectname$
{
    /// <summary>
    /// Разделяемый класс, который расширяет тип User и добавляет общие свойства и методы.
    /// Эти свойства и методы будут доступны как в серверном, так и в клиентском приложениях.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Возвращает отображаемое имя пользователя, которое по умолчанию равно значению свойства FriendlyName.
        /// Если свойство FriendlyName не задано, возвращается имя пользователя.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FriendlyName))
                {
                    return this.FriendlyName;
                }
                else
                {
                    return this.Name;
                }
            }
        }
    }
}
