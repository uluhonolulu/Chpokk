
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// 控制項中可見狀態的名稱和 Helper。
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// BusyIndicator 的 Busy 狀態。
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// BusyIndicator 的 Idle 狀態。
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// 忙碌狀態群組名稱。
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// BusyIndicator 的 Visible 狀態名稱。
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// BusyIndicator 的 Hidden 狀態名稱。
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// BusyDisplay 群組。
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
