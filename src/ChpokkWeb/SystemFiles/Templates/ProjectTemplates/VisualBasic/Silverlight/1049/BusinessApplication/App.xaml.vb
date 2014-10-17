Imports System
Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls


''' <summary>
''' Главный класс приложения <see cref="Application"/>.
''' </summary>
Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' Создает новый экземпляр класса <see cref="App"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        ' Создание объекта WebContext и добавление его в коллекцию ApplicationLifetimeObjects.
        ' После этого он будет доступен как WebContext.Current.
        Dim webContext As New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        Me.ApplicationLifetimeObjects.Add(webContext)
    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
        ' Это позволит привязывать элементы управления в файлах XAML к свойствам WebContext.Current
        Me.Resources.Add("WebContext", WebContext.Current)

        ' Пользователь будет проходить проверку подлинности автоматически, если используется проверка подлинности Windows или в предыдущий раз был установлен флажок "Запомнить пароль".
        WebContext.Current.Authentication.LoadUser(AddressOf Me.Application_UserLoaded, Nothing)

        ' Отображает пользовательский интерфейс во время выполнения операции LoadUser
        Me.InitializeRootVisual()
    End Sub

    ''' <summary>
    ''' Вызывается после завершения операции <see cref="LoadUserOperation"/>. 
    ''' Обработчик этого события служит для переключения из "пользовательского интерфейса загрузки", созданного в <see cref="InitializeRootVisual"/>, в "пользовательский интерфейс приложения".
    ''' </summary>
    Private Sub Application_UserLoaded(ByVal operation As LoadUserOperation)
        If operation.HasError Then
            ErrorWindow.CreateNew(operation.Error)
            operation.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' Инициализирует свойство <see cref="Application.RootVisual"/>. 
    ''' До завершения выполнения операции LoadUser будет отображен первоначальный пользовательский интерфейс.
    ''' Операция LoadUser осуществляет автоматический вход пользователя, если он использует проверку подлинности Windows или в предыдущий раз был установлен флажок "Запомнить пароль".
    ''' </summary>
    Protected Overridable Sub InitializeRootVisual()
        Dim busyIndicator As New $safeprojectname$.Controls.BusyIndicator()
        busyIndicator.Content = New MainPage()
        busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch
        busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch

        Me.RootVisual = busyIndicator
    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)
        ' Если приложение запущено не в режиме отладки, сообщение об исключении выводится в элементе управления ChildWindow.
        If Not System.Diagnostics.Debugger.IsAttached Then
            ' ПРИМЕЧАНИЕ. Это позволит приложению продолжить выполнение после возникновения необработанного исключения. 
            ' Для производственных приложений такую обработку ошибок необходимо заменить кодом, который будет сообщать об ошибке на веб-узле и прекращать работу приложения.
            e.Handled = True
            ErrorWindow.CreateNew(e.ExceptionObject)
        End If
    End Sub
End Class