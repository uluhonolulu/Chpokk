namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// 对 <see cref="User"/> 类的扩展。
    /// </summary>
    public partial class User
    {
        #region 使 DisplayName 可绑定

        /// <summary>
        /// 用于在 <see cref="User.DisplayName"/> 更改时生成属性更改通知的 <c>OnPropertyChanged</c> 方法重写。
        /// </summary>
        /// <param name="e">属性更改事件参数。</param>
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
