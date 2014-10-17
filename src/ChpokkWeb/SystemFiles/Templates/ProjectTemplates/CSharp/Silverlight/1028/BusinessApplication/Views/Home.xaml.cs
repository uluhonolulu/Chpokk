namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// 應用程式的首頁。
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// 建立新 <see cref="Home"/> 執行個體。
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// 於使用者巡覽到此頁面時執行。
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}