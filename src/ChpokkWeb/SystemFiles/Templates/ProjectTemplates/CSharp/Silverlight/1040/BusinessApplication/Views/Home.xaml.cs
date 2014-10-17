namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Home page dell'applicazione.
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// Crea una nuova istanza di <see cref="Home"/>.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// Viene eseguito quando l'utente passa a questa pagina.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}