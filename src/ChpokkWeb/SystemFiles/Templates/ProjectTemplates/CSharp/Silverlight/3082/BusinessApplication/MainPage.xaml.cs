namespace $safeprojectname$
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using $safeprojectname$.LoginUI;

    /// <summary>
    /// Clase <see cref="UserControl"/> que proporciona la IU principal de la aplicación.
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Crea una nueva instancia de <see cref="MainPage"/>.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Una vez que el Frame navegue, asegúrese de que el <see cref="HyperlinkButton"/> que representa a la página actual esté seleccionado
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
        /// Si se produce un error durante la navegación, mostrar una ventana de error
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }
    }
}