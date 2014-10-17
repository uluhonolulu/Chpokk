namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Página de inicio de la aplicación.
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// Crea una nueva instancia de <see cref="Home"/>.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// Se ejecuta cuando el usuario navega a esta page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}