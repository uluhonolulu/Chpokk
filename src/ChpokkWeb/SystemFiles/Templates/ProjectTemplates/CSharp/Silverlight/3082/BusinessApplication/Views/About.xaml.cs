namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Clase <see cref="Page"/> para presentar información sobre la aplicación actual.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Crea una nueva instancia de la clase <see cref="About"/>.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// Se ejecuta cuando el usuario navega a esta página.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}