namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// <see cref="User"/> クラスの拡張です。
    /// </summary>
    public partial class User
    {
        #region DisplayName をバインド可能にします

        /// <summary>
        /// <see cref="User.DisplayName"/> が変更された場合にプロパティの変更通知を生成する <c>OnPropertyChanged</c> メソッドのオーバーライドです。
        /// </summary>
        /// <param name="e">プロパティ変更イベントの引数です。</param>
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
