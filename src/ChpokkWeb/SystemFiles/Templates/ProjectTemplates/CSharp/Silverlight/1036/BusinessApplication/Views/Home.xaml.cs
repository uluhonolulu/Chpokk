namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Page d'accueil de l'application.
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// Crée une nouvelle instance <see cref="Home"/>.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// S'exécute lorsque l'utilisateur navigue vers cette page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}