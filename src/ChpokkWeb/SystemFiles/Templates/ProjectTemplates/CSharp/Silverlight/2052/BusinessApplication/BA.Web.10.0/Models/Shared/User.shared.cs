namespace $safeprojectname$
{
    /// <summary>
    /// 对 User 类型进行了扩展的分部类，添加了共享属性和方法。
    /// 这些属性和方法对服务器应用程序和客户端应用程序都可用。
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// 返回用户显示名称，默认情况下为用户的 FriendlyName。
        /// 如果未设置 FriendlyName，则返回 UserName。
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
