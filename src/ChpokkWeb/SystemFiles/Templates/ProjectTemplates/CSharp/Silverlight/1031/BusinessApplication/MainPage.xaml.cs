namespace $safeprojectname$
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using $safeprojectname$.LoginUI;

    /// <summary>
    /// <see cref="UserControl"/> Klasse, die die Haupt-Benutzeroberfläche für die Anwendung bereitstellt.
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="MainPage"/>.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Nach der Navigation durch den Frame sicherstellen, dass der <see cref="HyperlinkButton"/> ausgewählt ist, der die aktuelle Seite darstellt
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
        /// Ein Fehlerfenster anzeigen, wenn während der Navigation ein Fehler auftritt
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }
    }
}