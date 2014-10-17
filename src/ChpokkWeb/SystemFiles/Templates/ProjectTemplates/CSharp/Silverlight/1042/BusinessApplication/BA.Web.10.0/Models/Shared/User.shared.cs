namespace $safeprojectname$
{
    /// <summary>
    /// 공유 속성 및 메서드를 추가하는 User 형식을 확장하는 Partial 클래스입니다.
    /// 이러한 속성과 메서드는 서버 응용 프로그램과 클라이언트 응용 프로그램 모두에서 사용할 수 있습니다.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// 사용자 표시 이름(기본값: FriendlyName)을 반환합니다.
        /// FriendlyName을 설정하지 않은 경우 사용자 이름이 반환됩니다.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FriendlyName))
                {
                    return this.FriendlyName;
                }
                else
                {
                    return this.Name;
                }
            }
        }
    }
}
