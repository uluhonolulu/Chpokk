
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// Nombres y aplicaciones auxiliares para los estados visuales de los controles.
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// Estado Busy de BusyIndicator.
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// Estado Idle de BusyIndicator.
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// Nombre de grupo de estados de ocupado.
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// Nombre de estado Visible de BusyIndicator.
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// Nombre de estado Hidden de BusyIndicator.
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// Grupo BusyDisplay.
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
