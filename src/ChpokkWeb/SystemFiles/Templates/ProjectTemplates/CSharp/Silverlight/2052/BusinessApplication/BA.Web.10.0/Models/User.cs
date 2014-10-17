namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// 包含有关已通过身份验证的用户的信息的类。
    /// </summary>
    public partial class User : UserBase
    {
        //// 注意: 可以添加配置文件属性以在 Silverlight 应用程序中使用。
        //// 若要启用配置文件，请编辑 web.config 文件的相应部分。
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// 获取和设置用户的友好名称。
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
