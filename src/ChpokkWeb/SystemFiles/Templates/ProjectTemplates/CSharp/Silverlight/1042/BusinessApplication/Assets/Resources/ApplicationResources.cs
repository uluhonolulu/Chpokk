namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// 컨트롤 속성을 XAML의 리소스 문자열에 바인딩할 수 있도록 강력한 형식의 리소스 클래스에 대한 액세스를 래핑합니다.
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// <see cref="ApplicationStrings"/>를 가져옵니다.
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// <see cref="ErrorResources"/>를 가져옵니다.
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
