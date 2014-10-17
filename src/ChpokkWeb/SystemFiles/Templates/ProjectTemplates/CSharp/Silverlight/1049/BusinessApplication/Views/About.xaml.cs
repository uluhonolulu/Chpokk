namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Класс <see cref="Page"/> представляет сведения о текущем приложении.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="About"/>.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// Выполняется, когда пользователь переходит на эту страницу.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}