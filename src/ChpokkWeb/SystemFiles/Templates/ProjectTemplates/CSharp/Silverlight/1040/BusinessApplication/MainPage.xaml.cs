namespace $safeprojectname$
{
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Navigation;
    using $safeprojectname$.LoginUI;

    /// <summary>
    /// Classe <see cref="UserControl"/> che fornisce l'interfaccia utente principale per l'applicazione.
    /// </summary>
    public partial class MainPage : UserControl
    {
        /// <summary>
        /// Crea una nuova istanza di <see cref="MainPage"/>.
        /// </summary>
        public MainPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Dopo la visualizzazione del Frame, verificare che sia selezionato l'oggetto <see cref="HyperlinkButton"/> che rappresenta la pagina corrente
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
        /// Se si verifica un errore durante l'esplorazione, mostrare una finestra di errore
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            ErrorWindow.CreateNew(e.Exception);
        }
    }
}