namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// 現在のアプリケーションに関する情報を表す <see cref="Page"/> クラスです。
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// <see cref="About"/> クラスの新しいインスタンスを作成します。
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// ユーザーがこのページに移動するときに実行します。
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}