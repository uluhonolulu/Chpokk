namespace $safeprojectname$
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using $safeprojectname$.LoginUI;

    /// <summary>
    /// 为应用程序提供主 UI 的 <see cref="UserControl"/> 类。
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// 创建新 <see cref="MainPage"/> 实例。
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 在 Frame 导航之后，请确保选中表示当前页的 <see cref="HyperlinkButton"/>
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
        /// 如果导航过程中出现错误，则显示错误窗口
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }
    }
}