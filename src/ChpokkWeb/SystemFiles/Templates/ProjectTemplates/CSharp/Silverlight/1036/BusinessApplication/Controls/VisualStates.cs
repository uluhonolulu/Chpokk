
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// Noms et programmes d'assistance pour les états visuels dans les contrôles.
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// État Busy pour BusyIndicator.
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// État Idle pour BusyIndicator.
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// Nom de groupe d'états Busy.
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// Nom d'état Visible pour BusyIndicator.
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// Nom d'état Hidden pour BusyIndicator.
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// Groupe BusyDisplay.
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
