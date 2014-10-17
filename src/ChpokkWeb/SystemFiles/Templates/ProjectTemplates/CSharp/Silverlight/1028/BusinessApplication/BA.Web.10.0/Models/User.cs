namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// 包含已驗證使用者相關資訊的類別。
    /// </summary>
    public partial class User : UserBase
    {
        //// 注意: 可以加入設定檔屬性以便在 Silverlight 應用程式中使用。
        //// 若要啟用設定檔，請編輯 web.config 檔案的適當區段。
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// 取得和設定使用者的易記名稱。
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
