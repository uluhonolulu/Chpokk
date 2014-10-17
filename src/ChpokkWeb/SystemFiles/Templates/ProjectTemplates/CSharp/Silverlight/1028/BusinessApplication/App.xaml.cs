namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// 主要 <see cref="Application"/> 類別。
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 建立新 <see cref="App"/> 執行個體。
        /// </summary>
        public App()
        {
            InitializeComponent();

            // 建立 WebContext 並且將它加入到 ApplicationLifetimeObjects 集合。
            // 然後它會以 WebContext.Current 的形式提供使用。
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 這將可讓您將 XAML 中的控制項繫結到 WebContext.Current 屬性。
            this.Resources.Add("WebContext", WebContext.Current);

            // 如果使用 Windows 驗證，或使用者在上次登入嘗試選擇了 [讓我保持登入]，它將會自動驗證使用者。
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // 於 LoadUser 進行中向使用者顯示某些 UI
            this.InitializeRootVisual();
        }

        /// <summary>
        /// 於 <see cref="LoadUserOperation"/> 完成時叫用。
        /// 使用這個事件處理常式從您在 <see cref="InitializeRootVisual"/> 中建立的 [使用者介面載入中] 切換為 [應用程式使用者介面]。
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
        /// 初始化 <see cref="Application.RootVisual"/> 屬性。
        /// 在 LoadUser 作業完成前將顯示初始 UI。
        /// 如果使用 Windows 驗證，或使用者在上次登入嘗試選擇了 [讓我保持登入]，LoadUser 作業將會讓使用者自動登入。
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
            // 如果應用程式是在偵錯工具之外執行，便使用 ChildWindow 控制項回報例外狀況。
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // 注意: 這樣會讓應用程式在擲回例外狀況 (但未處理) 之後繼續執行。
                // 對於實際執行的應用程式，這項錯誤處理將由會向網站回報錯誤及停止應用程式的錯誤處理取代。
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}