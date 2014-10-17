
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// 컨트롤의 표시 상태에 대한 이름 및 도우미입니다.
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// BusyIndicator에 대한 Busy 상태입니다.
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// BusyIndicator에 대한 Idle 상태입니다.
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// 사용 중 상태 그룹 이름입니다.
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// BusyIndicator에 대한 Visible 상태 이름입니다.
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// BusyIndicator에 대한 Hidden 상태 이름입니다.
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// BusyDisplay 그룹입니다.
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
