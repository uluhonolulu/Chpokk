
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// Namen und Helfer für visuelle Zustände in den Steuerelementen.
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// Der Zustand "Busy" für BusyIndicator.
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// Der Zustand "Idle" für BusyIndicator.
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// Gruppenname der Zustände "Busy".
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// Name des Zustands "Visible" für BusyIndicator.
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// Name des Zustands "Hidden" für BusyIndicator.
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// Gruppe "BusyDisplay"
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
