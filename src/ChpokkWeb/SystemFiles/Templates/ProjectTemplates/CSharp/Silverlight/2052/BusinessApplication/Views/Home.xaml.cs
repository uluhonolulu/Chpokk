namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// 应用程序的主页。
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// 创建新 <see cref="Home"/> 实例。
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// 当用户导航到此页面时执行。
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}