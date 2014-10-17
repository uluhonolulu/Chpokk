namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// 인증된 사용자에 대한 정보를 포함하는 클래스입니다.
    /// </summary>
    public partial class User : UserBase
    {
        //// 참고: Silverlight 응용 프로그램에 사용할 프로필 속성을 추가할 수 있습니다.
        //// 프로필을 사용하려면 web.config 파일에서 해당 섹션을 편집하십시오.
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// 사용자의 표시 이름을 가져오거나 설정합니다.
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
