namespace $safeprojectname$
{
    /// <summary>
    /// 共有プロパティおよびメソッドを追加する User 型を拡張する部分クラスです。
    /// これらのプロパティおよびメソッドは、サーバー アプリケーションとクライアント アプリケーションの両方で使用できます。
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// ユーザーの表示名 (既定ではユーザーの FriendlyName) を返します。
        /// FriendlyName が設定されていない場合は、ユーザー名が返されます。
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
