namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Расширения класса <see cref="User"/>.
    /// </summary>
    public partial class User
    {
        #region Делает DisplayName привязываемым

        /// <summary>
        /// Переопределение метода <c>OnPropertyChanged</c>, который вызывает уведомления об изменении свойства при изменении <see cref="User.DisplayName"/>.
        /// </summary>
        /// <param name="e">Аргументы события изменения свойств.</param>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);

            if (e.PropertyName == "Name" || e.PropertyName == "FriendlyName")
            {
                this.RaisePropertyChanged("DisplayName");
            }
        }
        #endregion
    }
}
