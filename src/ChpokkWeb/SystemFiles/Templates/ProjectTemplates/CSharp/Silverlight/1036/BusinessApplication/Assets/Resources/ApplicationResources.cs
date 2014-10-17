namespace $safeprojectname$
{
    using System;
    using System.ComponentModel;
    using System.Globalization;
    using System.Windows.Browser;

    /// <summary>
    /// Inclut dans un wrapper l'accès aux classes de ressources fortement typées de sorte que vous puissiez lier des propriétés de contrôle à des chaînes de ressource en XAML.
    /// </summary>
    public sealed class ApplicationResources
    {
        private static readonly ApplicationStrings applicationStrings = new ApplicationStrings();
        private static readonly ErrorResources errorResources = new ErrorResources();

        /// <summary>
        /// Obtient le <see cref="ApplicationStrings"/>.
        /// </summary>
        public ApplicationStrings Strings
        {
            get { return applicationStrings; }
        }

        /// <summary>
        /// Obtient le <see cref="ErrorResources"/>.
        /// </summary>
        public ErrorResources Errors
        {
            get { return errorResources; }
        }
    }
}
