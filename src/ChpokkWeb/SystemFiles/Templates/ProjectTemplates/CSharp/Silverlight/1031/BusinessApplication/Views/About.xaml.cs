namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// <see cref="Page"/> Klasse zur Bereitstellung von Informationen über die aktuelle Anwendung.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Erstellt eine neue Instanz der <see cref="About"/>-Klasse.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// Wird ausgeführt, wenn der Benutzer auf diese Seite navigiert.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}