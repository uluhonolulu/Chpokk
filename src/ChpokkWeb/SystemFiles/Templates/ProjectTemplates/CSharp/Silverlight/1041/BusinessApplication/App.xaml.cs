namespace $safeprojectname$
{
    using System;
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Client.ApplicationServices;
    using System.Windows;
    using System.Windows.Controls;

    /// <summary>
    /// メインの <see cref="Application"/> クラスです。
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 新しい <see cref="App"/> インスタンスを作成します。
        /// </summary>
        public App()
        {
            InitializeComponent();

            // WebContext を作成し、ApplicationLifetimeObjects コレクションに追加します。
            // その後、WebContext.Current として使用できるようになります。
            WebContext webContext = new WebContext();
            webContext.Authentication = new FormsAuthentication();
            //webContext.Authentication = new WindowsAuthentication();
            this.ApplicationLifetimeObjects.Add(webContext);
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            // これにより、XAML 内のコントロールを WebContext.Current プロパティにバインドできます。
            this.Resources.Add("WebContext", WebContext.Current);

            // これにより、Windows 認証を使用している場合、またはユーザーが前回のログイン試行時に [サインアウトしない] を選択した場合、ユーザーが自動的に認証されます。
            WebContext.Current.Authentication.LoadUser(this.Application_UserLoaded, null);

            // LoadUser の実行中に、ユーザーにいくつかの UI を表示します
            this.InitializeRootVisual();
        }

        /// <summary>
        /// <see cref="LoadUserOperation"/> が完了したときに呼び出されました。
        /// このイベント ハンドラーは、<see cref="InitializeRootVisual"/> で作成した "読み込み UI" から "アプリケーション UI" に切り替える場合に使用します。
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
        /// <see cref="Application.RootVisual"/> プロパティを初期化します。
        /// LoadUser 操作が完了する前に、最初の UI が表示されます。
        /// Windows 認証を使用している場合、またはユーザーが前回のログイン時に [サインアウトしない] を選択した場合、LoadUser 操作によりユーザーが自動的にログインされます。
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
            // デバッガー外部でアプリケーションが実行されている場合は、ChildWindow コントロールを使用して例外を報告します。
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                // メモ: これにより、例外がスローされて処理されていない場合でも、アプリケーションを引き続き実行できます。
                // 実稼働アプリケーションでは、このエラー処理は、Web サイトにエラーを報告し、アプリケーションを停止する処理に置換する必要があります。
                e.Handled = true;
                ErrorWindow.CreateNew(e.ExceptionObject);
            }
        }
    }
}