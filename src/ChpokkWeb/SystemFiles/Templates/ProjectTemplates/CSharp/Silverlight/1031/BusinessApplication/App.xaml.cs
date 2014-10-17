namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Haupt <see cref="Application"/>-Klasse.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="App"/>.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Einen WebContext erstellen und der Sammlung ApplicationLifetimeObjects hinzufügen.
            // Dieser ist anschließend als WebContext.Current verfügbar.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // So können Steuerelemente in XAML an WebContext.Current-Eigenschaften gebunden werden.
            this.Resources.Add("WebContext", WebContext.Current);

            // Dadurch wird ein Benutzer automatisch authentifiziert, wenn er die Windows-Authentifizierung benutzt oder er bei einer früheren Anmeldung die Option "Angemeldet bleiben" ausgewählt hat.
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // Dem Benutzer die Benutzeroberfläche anzeigen, während LoadUser ausgeführt wird
            this.InitializeRootVisual();
        }

        /// <summary>
        /// Wird beim Abschluss von <see cref="LoadUserOperation"/> aufgerufen.
        /// Nutzen Sie diesen Ereignishandler, um von der "Ladebenutzeroberfläche", die in <see cref="InitializeRootVisual"/> erstellt wurde, auf die "Anwendungsbenutzeroberfläche" umzuschalten.
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
        /// Initialisiert die Eigenschaft <see cref="Application.RootVisual"/>.
        /// Die ursprüngliche Benutzeroberfläche wird angezeigt, bevor der LoadUser-Vorgang abgeschlossen ist.
        /// Durch den LoadUser-Vorgang wird ein Benutzer automatisch angemeldet, wenn er die Windows-Authentifizierung benutzt oder er bei einer früheren Anmeldung die Option "Angemeldet bleiben" ausgewählt hat.
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
            // Falls die Anwendung außerhalb des Debuggers ausgeführt wird, melden Sie die Ausnahme mithilfe eines ChildWindow-Steuerelements.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // HINWEIS: So kann die Anwendung weiterhin ausgeführt werden, nachdem eine Ausnahme ausgelöst, aber nicht behandelt wurde. 
                // Bei Produktionsanwendungen sollte diese Fehlerbehandlung durch eine Anwendung ersetzt werden, die den Fehler der Website meldet und die Anwendung beendet.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}