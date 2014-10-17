namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Classe <see cref="Page"/> permettant de présenter les informations relatives à l'application actuelle.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Crée une nouvelle instance de la classe <see cref="About"/>.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// S'exécute lorsque l'utilisateur navigue vers cette page.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}