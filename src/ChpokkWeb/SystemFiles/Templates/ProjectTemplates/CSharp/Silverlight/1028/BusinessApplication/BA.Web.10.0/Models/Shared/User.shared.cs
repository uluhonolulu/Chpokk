namespace $safeprojectname$
{
    /// <summary>
    /// 新增了共用屬性和方法之延伸 User 型別的部分類別。
    /// 這些屬性和方法可同時供伺服器應用程式和用戶端應用程式使用。
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// 傳回使用者顯示名稱 (預設為它的 FriendlyName)，
        /// 如果沒有設定 FriendlyName，則會傳回使用者名稱。
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
