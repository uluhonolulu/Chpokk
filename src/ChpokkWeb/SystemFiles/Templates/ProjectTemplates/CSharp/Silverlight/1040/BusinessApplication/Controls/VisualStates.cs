
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// Nomi e supporti per gli stati di visualizzazione nei controlli.
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// Stato Busy per BusyIndicator.
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// Stato Idle per BusyIndicator.
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// Nome gruppo di stati Busy.
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// Nome stato Visible per BusyIndicator.
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// Nome stato Hidden per BusyIndicator.
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// Gruppo BusyDisplay.
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
