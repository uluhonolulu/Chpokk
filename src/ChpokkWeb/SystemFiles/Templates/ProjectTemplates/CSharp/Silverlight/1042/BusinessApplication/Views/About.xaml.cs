namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// 현재 응용 프로그램에 대한 정보를 제공하기 위한 <see cref="Page"/> 클래스입니다.
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// <see cref="About"/> 클래스의 새 인스턴스를 만듭니다.
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// 사용자가 이 페이지를 탐색할 때 실행됩니다.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}