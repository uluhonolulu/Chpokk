namespace $safeprojectname$
{
    using System.Windows.Controls;
    using System.Windows.Navigation;

    /// <summary>
    /// 呈現目前應用程式相關資訊的 <see cref="Page"/> 類別。
    /// </summary>
    public partial class About : Page
    {
        /// <summary>
        /// 建立 <see cref="About"/> 類別的新執行個體。
        /// </summary>
        public About()
        {
            InitializeComponent();

            this.Title = ApplicationStrings.AboutPageTitle;
        }

        /// <summary>
        /// 於使用者巡覽到此頁面時執行。
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}