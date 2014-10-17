namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// アプリケーションのホーム ページです。
    /// </summary>
    public partial class Home : Page
    {
        /// <summary>
        /// 新しい <see cref="Home"/> インスタンスを作成します。
        /// </summary>
        public Home()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.HomePageTitle;
        }

        /// <summary>
        /// ユーザーがこのページに移動するときに実行します。
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}