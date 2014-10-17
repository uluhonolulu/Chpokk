namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// Обертка доступа к классам ресурсов со строгой типизацией, чтобы можно было привязывать свойства элементов управления к строкам ресурсов в XAML.
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// Возвращает <see cref="ApplicationStrings"/>.
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// Возвращает <see cref="ErrorResources"/>.
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
