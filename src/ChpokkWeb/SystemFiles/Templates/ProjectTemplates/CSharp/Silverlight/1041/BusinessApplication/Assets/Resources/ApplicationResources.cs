namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// コントロールのプロパティを XAML 内のリソース文字列にバインドできるように、厳密に型指定されたリソース クラスへのアクセスをラップします。
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// <see cref="ApplicationStrings"/> を取得します。
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// <see cref="ErrorResources"/> を取得します。
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
