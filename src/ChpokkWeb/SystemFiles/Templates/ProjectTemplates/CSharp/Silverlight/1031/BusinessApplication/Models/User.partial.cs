namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Erweiterungen der Klasse <see cref="User"/>.
    /// </summary>
    public partial class User
    {
        #region Macht DisplayName bindbar

        /// <summary>
        /// Überschreiben der Methode <c>OnPropertyChanged</c>, die Benachrichtigungen für die Änderung der Eigenschaft erzeugt, wenn sich <see cref="User.DisplayName"/> ändert.
        /// </summary>
        /// <param name="e">Die Ereignisargumente für die Änderung der Eigenschaft.</param>
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
