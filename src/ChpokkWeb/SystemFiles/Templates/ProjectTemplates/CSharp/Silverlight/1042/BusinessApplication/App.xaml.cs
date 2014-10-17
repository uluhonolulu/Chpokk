namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// 기본 <see cref="Application"/> 클래스입니다.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 새 <see cref="App"/> 인스턴스를 만듭니다.
        /// </summary>
        public App()
        {
            InitializeComponent();

            // WebContext를 만들어 ApplicationLifetimeObjects 컬렉션에 추가합니다.
            // 그런 다음 WebContext.Current로 사용할 수 있습니다.
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // XAML의 컨트롤을 WebContext.Current 속성에 바인딩할 수 있도록 합니다.
            this.Resources.Add("WebContext", WebContext.Current);

            // Windows 인증을 사용하거나 이전 로그인 시도에서 "로그인 유지"를 선택한 경우 사용자가 자동으로 인증됩니다.
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // LoadUser 진행 중에 사용자에게 일부 UI를 표시합니다.
            this.InitializeRootVisual();
        }

        /// <summary>
        /// <see cref="LoadUserOperation"/>이 완료되면 호출됩니다.
        /// 이 이벤트 처리기를 사용하여 <see cref="InitializeRootVisual"/>에서 만든 "로딩 UI"를 "응용 프로그램 UI"로 전환할 수 있습니다.
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
        /// <see cref="Application.RootVisual"/> 속성을 초기화합니다.
        /// 초기 UI는 LoadUser 작업이 완료되기 전에 표시됩니다.
        /// Windows 인증을 사용하거나 이전 로그인에서 "로그인 유지"를 선택한 경우 LoadUser 작업을 수행하면 사용자가 자동으로 로그인됩니다.
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
            // 응용 프로그램이 디버거 외부에서 실행 중인 경우 ChildWindow 컨트롤을 사용하여 예외를 보고합니다.
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // 참고: 예외가 throw된 이후에 처리되지 않은 상태에서도 응용 프로그램을 계속 실행할 수 있습니다. 
                // 프로덕션 응용 프로그램의 경우 이 오류 처리는 웹 사이트에 오류를 보고하고 응용 프로그램을 중지하는 것으로 대체해야 합니다.
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}