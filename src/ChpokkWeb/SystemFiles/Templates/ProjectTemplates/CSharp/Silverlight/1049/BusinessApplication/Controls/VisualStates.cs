
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// Имена и помощники для визуальных состояний в элементах управления.
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// Состояние занятости (Busy) для элемента BusyIndicator.
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// Неактивное состояние (Idle) для элемента BusyIndicator.
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// Имя группы состояний занятости.
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// Имя состояния видимости (Visible) для элемента BusyIndicator.
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// Имя состояния невидимости (Hidden) для элемента BusyIndicator.
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// Группа BusyDisplay.
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
