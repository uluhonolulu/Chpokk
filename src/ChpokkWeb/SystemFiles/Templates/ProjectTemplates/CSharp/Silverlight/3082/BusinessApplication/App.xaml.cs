namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Clase <see cref="Application"/> principal.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Crea una nueva instancia de <see cref="App"/>.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Cree un WebContext y agréguelo a la colección ApplicationLifetimeObjects.
            // Esto entonces estará disponible como WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Esto permitirá enlazar controles de XAML a propiedades WebContext.Current.
            this.Resources.Add("WebContext", WebContext.Current);

            // Esto autenticará automáticamente a un usuario si utiliza autenticación de Windows o si ha seleccionado "Mantener la sesión iniciada" en un intento de inicio de sesión anterior.
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // Mostrar alguna IU al usuario mientras LoadUser está en curso
            this.InitializeRootVisual();
        }

        /// <summary>
        /// Se invoca cuando se completa <see cref="LoadUserOperation"/>.
        /// Utilice este controlador de eventos para pasar de la "IU de carga" que creó en <see cref="InitializeRootVisual"/> a la "IU de la aplicación".
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
        /// Inicializa la propiedad <see cref="Application.RootVisual"/>.
        /// La IU inicial se mostrará antes de que se haya completado la operación LoadUser.
        /// La operación LoadUser hará que el usuario inicie sesión automáticamente si utiliza autenticación de Windows o si ha seleccionado la opción "Mantener la sesión iniciada" en un inicio de sesión anterior.
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
            // Si la aplicación se está ejecutando fuera del depurador, notifique la excepción mediante un control ChildWindow.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // NOTA: esto permitirá que la aplicación se siga ejecutando después de que se haya iniciado, pero no controlado, una exception. 
                // En las aplicaciones de producción, este control de errores debería sustituirse por algo que notifique el error al sitio web y detenga la aplicación.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}