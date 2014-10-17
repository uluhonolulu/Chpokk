namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Estensioni alla classe <see cref="User"/>.
    /// </summary>
    public partial class User
    {
        #region Rendi DisplayName associabile

        /// <summary>
        /// Eseguire l'override del metodo <c>OnPropertyChanged</c> che genera notifiche di modifica delle proprietà in caso di modifica di <see cref="User.DisplayName"/>.
        /// </summary>
        /// <param name="e">Argomenti dell'evento di modifica delle proprietà.</param>
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
