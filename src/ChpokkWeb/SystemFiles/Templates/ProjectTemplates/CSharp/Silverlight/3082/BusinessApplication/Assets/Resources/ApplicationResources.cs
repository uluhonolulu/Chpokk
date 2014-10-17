namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// Encapsula el acceso a las clases de recursos fuertemente tipadas de modo que se puedan enlazar las propiedades del control a las cadenas de recursos del XAML.
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// Obtiene las <see cref="ApplicationStrings"/>.
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// Obtiene los <see cref="ErrorResources"/>.
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
