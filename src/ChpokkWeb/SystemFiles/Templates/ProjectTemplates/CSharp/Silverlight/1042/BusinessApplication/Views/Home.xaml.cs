namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// 응용 프로그램의 홈 페이지입니다.
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// 새 <see cref="Home"/> 인스턴스를 만듭니다.
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// 사용자가 이 페이지를 탐색할 때 실행됩니다.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}