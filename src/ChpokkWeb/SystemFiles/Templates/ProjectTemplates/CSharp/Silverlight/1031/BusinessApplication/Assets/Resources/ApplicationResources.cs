namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// Umschließt den Zugriff auf stark typisierte Ressourcenklassen, damit Steuerelementeigenschaften an Ressourcenzeichenfolgen in XAML gebunden werden können.
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// Ruft die <see cref="ApplicationStrings"/> ab.
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// Ruft die <see cref="ErrorResources"/> ab.
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
