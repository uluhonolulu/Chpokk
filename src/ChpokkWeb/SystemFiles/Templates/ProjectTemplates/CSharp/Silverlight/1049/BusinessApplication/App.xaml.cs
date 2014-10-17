namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// Главный класс приложения <see cref="Application"/>.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="App"/>.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // Создание объекта WebContext и добавление его в коллекцию ApplicationLifetimeObjects.
            // После этого он будет доступен как WebContext.Current.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // Это позволит привязывать элементы управления в файлах XAML к свойствам WebContext.Current.
            this.Resources.Add("WebContext", WebContext.Current);

            // Пользователь будет проходить проверку подлинности автоматически, если используется проверка подлинности Windows или в предыдущий раз был установлен флажок "Запомнить пароль".
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // Отображает пользовательский интерфейс во время выполнения операции LoadUser
            this.InitializeRootVisual();
        }

        /// <summary>
        /// Вызывается после завершения операции <see cref="LoadUserOperation"/>.
        /// Обработчик этого события служит для переключения из "пользовательского интерфейса загрузки", созданного в <see cref="InitializeRootVisual"/>, в "пользовательский интерфейс приложения".
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
        /// Инициализирует свойство <see cref="Application.RootVisual"/>.
        /// До завершения выполнения операции LoadUser будет отображен первоначальный пользовательский интерфейс.
        /// Операция LoadUser осуществляет автоматический вход пользователя, если он использует проверку подлинности Windows или в предыдущий раз был установлен флажок "Запомнить пароль".
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
            // Если приложение запущено не в режиме отладки, сообщение об исключении выводится в элементе управления ChildWindow.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // ПРИМЕЧАНИЕ. Это позволит приложению продолжить выполнение после возникновения необработанного исключения (exception). 
                // Для производственных приложений такую обработку ошибок необходимо заменить кодом, который будет сообщать об ошибке на веб-узле и прекращать работу приложения.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}