namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Extensiones de la clase <see cref="User"/>.
    /// </summary>
    public partial class User
    {
        #region Convertir DisplayName en enlazable

        /// <summary>
        /// Invalidación del método <c>OnPropertyChanged</c> que genera notificaciones de cambio de propiedades cuando cambia <see cref="User.DisplayName"/>.
        /// </summary>
        /// <param name="e">Argumentos del evento de cambio de la propiedad</param>
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
