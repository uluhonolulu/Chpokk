
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// 控件中的可视状态的名称和帮助程序。
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// BusyIndicator 的 Busy 状态。
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// BusyIndicator 的 Idle 状态。
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// 繁忙状态组名称。
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// BusyIndicator 的 Visible 状态名称。
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// BusyIndicator 的 Hidden 状态名称。
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// BusyDisplay 组。
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
