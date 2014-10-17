namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// 將存取包裝到強型別資源類別，就可以將控制項屬性繫結到 XAML 中的資源字串。
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// 取得 <see cref="ApplicationStrings"/>。
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// 取得 <see cref="ErrorResources"/>。
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
