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
        /// Crée une nouvelle instance <see cref="App"/>.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Créer un WebContext et l'ajouter à la collection ApplicationLifetimeObjects.
            // Ce sera alors disponible en tant que WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Cela vous permet de lier des contrôles en XAML à des propriétés WebContext.Current.
            this.Resources.Add("WebContext", WebContext.Current);

            // Cette opération authentifie automatiquement un utilisateur lorsque l'authentification Windows est utilisée ou lorsque l'utilisateur a choisi « Maintenir la connexion » lors d'une précédente tentative de connexion.
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // Afficher certains éléments d'interface utilisateur à l'utilisateur pendant que LoadUser est en cours
            this.InitializeRootVisual();
        }

        /// <summary>
        /// Appelé lorsque le <see cref="LoadUserOperation"/> se termine.
        /// Utilisez ce gestionnaire d'événements pour basculer de l'« interface utilisateur de chargement » que vous avez créée dans <see cref="InitializeRootVisual"/> vers l'« interface utilisateur de l'application ».
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
        /// Initialise la propriété <see cref="Application.RootVisual"/>.
        /// L'interface utilisateur initiale s'affiche avant que l'opération LoadUser soit terminée.
        /// L'opération LoadUser entraîne la connexion automatique de l'utilisateur si l'authentification Windows est utilisée ou si l'utilisateur a sélectionné l'option « Maintenir la connexion » lors d'une précédente connexion.
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
            // Si l'application s'exécute en dehors du débogueur, signaler l'exception à l'aide d'un contrôle ChildWindow.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // REMARQUE : cela permet à l'application de continuer à s'exécuter après qu'une exception a été levée mais pas gérée. 
                // Pour des applications de production, cette gestion des erreurs doit être remplacée par un système qui signale l'erreur au site Web et arrête l'application.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}