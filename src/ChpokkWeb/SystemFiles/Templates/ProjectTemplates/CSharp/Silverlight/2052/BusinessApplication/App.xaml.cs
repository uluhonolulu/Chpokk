namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// 主 <see cref="Application"/> 类。
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 创建新 <see cref="App"/> 实例。
        /// </summary>
        public App()
        {
            InitializeComponent();

            // 创建 WebContext 并将其添加到 ApplicationLifetimeObjects 集合。
            // 这随后可以用作 WebContext.Current。
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // 这使您可以将 XAML 文件中的控件绑定到 WebContext.Current 属性。
            this.Resources.Add("WebContext", WebContext.Current);

            // 如果使用 Windows 身份验证，或如果用户在上次登录尝试时选择了“使我保持登录状态”，则会自动对用户进行身份验证。
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // 在执行 LoadUser 期间向用户显示一些 UI
            this.InitializeRootVisual();
        }

        /// <summary>
        /// 在 <see cref="LoadUserOperation"/> 完成时调用。 
        /// 使用此事件处理程序可从您在 <see cref="InitializeRootVisual"/> 中创建的“加载 UI”切换为“应用程序 UI”。
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
        /// 初始化 <see cref="Application.RootVisual"/> 属性。
        /// 在 LoadUser 操作完成之前将显示初始 UI。
        /// 如果使用 Windows 身份验证，或如果用户在上次登录尝试时选择了“使我保持登录状态”选项，则 LoadUser 操作会使用户自动登录。
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
            // 如果应用程序在调试器之外运行，则使用 ChildWindow 控件报告异常。
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // 注意: 这使应用程序可以在已引发异常但尚未处理该异常的情况下继续运行。 
                // 对于生产应用程序，应将此错误处理替换为向网站报告错误并停止应用程序。
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}