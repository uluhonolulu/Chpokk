
namespace $safeprojectname$.Controls
{
    /// <summary>
    /// コントロールの表示状態の名前およびヘルパーです。
    /// </summary>
    internal static class VisualStates
    {
        #region GroupBusyStatus
        /// <summary>
        /// BusyIndicator の Busy 状態です。
        /// </summary>
        public const string StateBusy = "Busy";

        /// <summary>
        /// BusyIndicator の Idle 状態です。
        /// </summary>
        public const string StateIdle = "Idle";

        /// <summary>
        /// ビジー状態のグループ名です。
        /// </summary>
        public const string GroupBusyStatus = "BusyStatusStates";
        #endregion

        #region GroupVisibility
        /// <summary>
        /// BusyIndicator の Visible 状態の名前です。
        /// </summary>
        public const string StateVisible = "Visible";

        /// <summary>
        /// BusyIndicator の Hidden 状態の名前です。
        /// </summary>
        public const string StateHidden = "Hidden";
        
        /// <summary>
        /// BusyDisplay グループです。
        /// </summary>
        public const string GroupVisibility = "VisibilityStates";
        #endregion
    }
}
