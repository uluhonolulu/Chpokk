namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// 認証されているユーザーに関する情報を含むクラスです。
    /// </summary>
    public partial class User : UserBase
    {
        //// メモ: Silverlight アプリケーションで使用するためにプロファイル プロパティを追加できます。
        //// プロファイルを有効にするには、web.config ファイルの該当する部分を編集してください。
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// ユーザーのフレンドリ名を取得および設定します。
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
