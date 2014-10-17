namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Classe <see cref="Page"/> per mostrare informazioni sull'applicazione corrente.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Crea una nuova istanza della classe <see cref="About"/>.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// Viene eseguito quando l'utente passa a questa pagina.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}