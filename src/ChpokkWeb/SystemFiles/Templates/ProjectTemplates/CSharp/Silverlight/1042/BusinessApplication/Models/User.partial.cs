namespace $safeprojectname$.Web
{
    using System;
    using System.ComponentModel;

    /// <summary>
    /// <see cref="User"/> 클래스에 대한 확장입니다.
    /// </summary>
    public partial class User
    {
        #region DisplayName을 바인딩 가능으로 지정

        /// <summary>
        /// <see cref="User.DisplayName"/>이 변경될 때 속성 변경 알림을 생성하는 <c>OnPropertyChanged</c> 메서드의 재정의입니다.
        /// </summary>
        /// <param name="e">속성 변경 이벤트 인수입니다.</param>
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
