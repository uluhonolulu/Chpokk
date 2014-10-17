namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// Esegue il wrapping dell'accesso alle classi di risorse fortemente tipizzate in modo da poter associare le proprietà dei controllo alle stringhe di risorse in XAML.
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// Ottiene l'oggetto <see cref="ApplicationStrings"/>.
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// Ottiene l'oggetto <see cref="ErrorResources"/>.
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
