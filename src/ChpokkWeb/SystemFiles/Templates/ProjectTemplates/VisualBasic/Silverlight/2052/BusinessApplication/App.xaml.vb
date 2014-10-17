Imports System
Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls


''' <summary>
''' 主 <see cref="Application"/> 类。
''' </summary>
Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' 创建新 <see cref="App"/> 实例。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        ' 创建 WebContext 并将其添加到 ApplicationLifetimeObjects 集合。
        ' 这随后可以用作 WebContext.Current。
        Dim webContext As New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        Me.ApplicationLifetimeObjects.Add(webContext)
    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
        ' 这会使您可以将 XAML 文件中的控件绑定到 WebContext.Current 属性。
        Me.Resources.Add("WebContext", WebContext.Current)

        ' 如果使用 Windows 身份验证，或如果用户在上次登录尝试时选择了“使我保持登录状态”，则会自动对用户进行身份验证。
        WebContext.Current.Authentication.LoadUser(AddressOf Me.Application_UserLoaded, Nothing)

        ' 在执行 LoadUser 期间向用户显示一些 UI
        Me.InitializeRootVisual()
    End Sub

    ''' <summary>
    ''' 在 <see cref="LoadUserOperation"/> 完成时调用。 
    ''' 使用此事件处理程序可从您在 <see cref="InitializeRootVisual"/> 中创建的“加载 UI”切换为“应用程序 UI”。
    ''' </summary>
    Private Sub Application_UserLoaded(ByVal operation As LoadUserOperation)
        If operation.HasError Then
            ErrorWindow.CreateNew(operation.Error)
            operation.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' 初始化 <see cref="Application.RootVisual"/> 属性。
    ''' 在 LoadUser 操作完成之前将显示初始 UI。
    ''' 如果使用 Windows 身份验证，或如果用户在上次登录尝试时选择了“使我保持登录状态”选项，则 LoadUser 操作会使用户自动登录。
    ''' </summary>
    Protected Overridable Sub InitializeRootVisual()
        Dim busyIndicator As New $safeprojectname$.Controls.BusyIndicator()
        busyIndicator.Content = New MainPage()
        busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch
        busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch

        Me.RootVisual = busyIndicator
    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)
        ' 如果应用程序在调试器之外运行，则使用 ChildWindow 控件报告异常。
        If Not System.Diagnostics.Debugger.IsAttached Then
            ' 注意: 这使应用程序可以在已引发异常但尚未处理该异常的情况下继续运行。
            ' 对于生产应用程序，应将此错误处理替换为向网站报告错误并停止应用程序。
            e.Handled = True
            ErrorWindow.CreateNew(e.ExceptionObject)
        End If
    End Sub
End Class