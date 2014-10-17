namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// <see cref="User"/> 類別的延伸模組。
    /// </summary>
    public partial class User
    {
        #region 將 DisplayName 設成可繫結

        /// <summary>
        /// 覆寫會於 <see cref="User.DisplayName"/> 變更時產生屬性變更通知的 <c>OnPropertyChanged</c> 方法。
        /// </summary>
        /// <param name="e">屬性變更事件引數。</param>
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
