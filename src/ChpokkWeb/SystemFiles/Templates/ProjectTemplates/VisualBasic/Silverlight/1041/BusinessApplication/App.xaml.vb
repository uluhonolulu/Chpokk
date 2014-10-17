Imports System
Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls


''' <summary>
''' メインの <see cref="Application"/> クラスです。
''' </summary>
Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' 新しい <see cref="App"/> インスタンスを作成します。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        ' WebContext を作成し、ApplicationLifetimeObjects コレクションに追加します。
        ' その後、WebContext.Current として使用できるようになります。
        Dim webContext As New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        Me.ApplicationLifetimeObjects.Add(webContext)
    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
        ' これにより、XAML ファイル内のコントロールを WebContext.Current プロパティにバインドできます
        Me.Resources.Add("WebContext", WebContext.Current)

        ' これにより、Windows 認証を使用している場合、またはユーザーが前回のログイン試行時に [サインアウトしない] を選択した場合、ユーザーが自動的に認証されます。
        WebContext.Current.Authentication.LoadUser(AddressOf Me.Application_UserLoaded, Nothing)

        ' LoadUser の実行中に、ユーザーにいくつかの UI を表示します
        Me.InitializeRootVisual()
    End Sub

    ''' <summary>
    ''' <see cref="LoadUserOperation"/> が完了したときに呼び出されました。
    ''' このイベント ハンドラーは、<see cref="InitializeRootVisual"/> で作成した "読み込み UI" から "アプリケーション UI" に切り替える場合に使用します。
    ''' </summary>
    Private Sub Application_UserLoaded(ByVal operation As LoadUserOperation)
        If operation.HasError Then
            ErrorWindow.CreateNew(operation.Error)
            operation.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' <see cref="Application.RootVisual"/> プロパティを初期化します。
    ''' LoadUser 操作が完了する前に、最初の UI が表示されます。
    ''' Windows 認証を使用している場合、またはユーザーが前回のログイン時に [サインアウトしない] を選択した場合、LoadUser 操作によりユーザーが自動的に記録されます。
    ''' </summary>
    Protected Overridable Sub InitializeRootVisual()
        Dim busyIndicator As New $safeprojectname$.Controls.BusyIndicator()
        busyIndicator.Content = New MainPage()
        busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch
        busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch

        Me.RootVisual = busyIndicator
    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)
        ' デバッガー外部でアプリケーションが実行されている場合は、ChildWindow コントロールを使用して例外を報告します。
        If Not System.Diagnostics.Debugger.IsAttached Then
            ' メモ: これにより、例外がスローされて処理されていない場合でも、アプリケーションを引き続き実行できます。
            ' 実稼働アプリケーションでは、このエラー処理は、Web サイトにエラーを報告し、アプリケーションを停止する処理に置換する必要があります。
            e.Handled = True
            ErrorWindow.CreateNew(e.ExceptionObject)
        End If
    End Sub
End Class