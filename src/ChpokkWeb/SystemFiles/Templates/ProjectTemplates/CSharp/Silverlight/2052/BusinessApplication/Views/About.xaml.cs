namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// 用于提供有关当前应用程序的信息的 <see cref="Page"/> 类。
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// 创建 <see cref="About"/> 类的新实例。
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// 当用户导航到此页面时执行。
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}