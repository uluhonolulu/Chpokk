namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// Домашняя страница приложения.
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="Home"/>.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// Выполняется, когда пользователь переходит на эту страницу.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}