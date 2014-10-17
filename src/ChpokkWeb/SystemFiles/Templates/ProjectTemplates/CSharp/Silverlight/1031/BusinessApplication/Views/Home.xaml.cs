namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Startseite der Anwendung.
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="Home"/>.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// Wird ausgeführt, wenn der Benutzer auf diese Seite navigiert.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}