namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// Extensions de la classe <see cref="User"/>.
    /// </summary>
    public partial class User
    {
        #region Rendre DisplayName Bindable

        /// <summary>
        /// Substitution de la méthode <c>OnPropertyChanged</c> qui génère des notifications de modification de propriété lorsque <see cref="User.DisplayName"/> change.
        /// </summary>
        /// <param name="e">Arguments de l'événement de modification de la propriété.</param>
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
