namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Classe <see cref="Application"/> principale.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Crea una nuova istanza di <see cref="App"/>.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Creare un oggetto WebContext e aggiungerlo al set ApplicationLifetimeObjects.
            // Tale oggetto sarà quindi disponibile come WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Ciò consentirà di associare i controlli presenti nei file XAML alle proprietà WebContext.Current.
            this.Resources.Add("WebContext", WebContext.Current);

            // In questo modo l'utente verrà autenticato automaticamente se viene utilizzata l'autenticazione di Windows o se è stata selezionata l'opzione "Mantieni l'accesso" durante un tentativo di accesso precedente.
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // Mostrare all'utente una parte dell'interfaccia utente durante l'operazione LoadUser
            this.InitializeRootVisual();
        }

        /// <summary>
        /// Richiamato al completamento di <see cref="LoadUserOperation"/>.
        /// Utilizzare questo gestore eventi per passare dall'interfaccia di caricamento creata in <see cref="InitializeRootVisual"/> all'interfaccia utente dell'applicazione.
        /// </summary>
        private void Application_UserLoaded(LoadUserOperation operation)
        {
            if (operation.HasError)
            {
                ErrorWindow.CreateNew(operation.Error);
                operation.MarkErrorAsHandled();
            }
        }

        /// <summary>
        /// Inizializza la proprietà <see cref="Application.RootVisual"/>.
        /// L'interaccia utente iniziale verrà visualizzata prima del completamento dell'operazione LoadUser.
        /// L'operazione LoadUser comporterà l'accesso automatico dell'utente se viene utilizzata l'autenticazione di Windows o se è stata selezionata l'opzione "Mantieni l'accesso" durante un accesso precedente.
        /// </summary>
        protected virtual void InitializeRootVisual()
        {
            $safeprojectname$.Controls.BusyIndicator busyIndicator = new $safeprojectname$.Controls.BusyIndicator();
            busyIndicator.Content = new MainPage();
            busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch;
            busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch;

            this.RootVisual = busyIndicator;
        }

        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            // Se l'applicazione viene eseguita all'esterno del debugger, segnalare l'eccezione mediante un controllo ChildWindow.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTA: in questo modo sarà possibile continuare l'esecuzione dell'applicazione dopo la generazione di un'eccezione, anche se non gestita. 
                // Per le applicazioni di produzione è consigliabile invece segnalare l'errore al sito Web e arrestare l'applicazione.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}