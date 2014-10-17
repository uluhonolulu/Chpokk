namespace $safeprojectname$
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using $safeprojectname$.LoginUI;

    /// <summary>
    /// 提供應用程式主要 UI 的 <see cref="UserControl"/> 類別。
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// 建立新 <see cref="MainPage"/> 執行個體。
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 於 Frame 巡覽之後，確認代表目前頁面的 <see cref="HyperlinkButton"/> 為選取狀態
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
            foreach (UIElement child in LinksStackPanel.Children)
            {
                HyperlinkButton hb = child as HyperlinkButton;
                if (hb != null && hb.NavigateUri != null)
                {
                    if (hb.NavigateUri.ToString().Equals(e.Uri.ToString()))
                    {
                        VisualStateManager.GoToState(hb, "ActiveLink", true);
                    }
                    else
                    {
                        VisualStateManager.GoToState(hb, "InactiveLink", true);
                    }
                }
            }
        }

        /// <summary>
        /// 如果巡覽期間發生錯誤，即顯示錯誤視窗
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }
    }
}