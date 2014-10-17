Imports System
Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls


''' <summary>
''' Clase <see cref="Application"/> principal.
''' </summary>
Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' Crea una nueva instancia de <see cref="App"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        ' Cree un WebContext y agréguelo a la colección ApplicationLifetimeObjects.
        ' Esto entonces estará disponible como WebContext.Current.
        Dim webContext As New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        Me.ApplicationLifetimeObjects.Add(webContext)
    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
        ' Esto permitirá enlazar controles de archivos XAML a propiedades WebContext.Current
        Me.Resources.Add("WebContext", WebContext.Current)

        ' Esto autenticará automáticamente a un usuario si utiliza autenticación de Windows o si ha seleccionado "Mantener la sesión iniciada" en un intento de inicio de sesión anterior.
        WebContext.Current.Authentication.LoadUser(AddressOf Me.Application_UserLoaded, Nothing)

        ' Mostrar alguna IU al usuario mientras LoadUser está en curso
        Me.InitializeRootVisual()
    End Sub

    ''' <summary>
    ''' Se invoca cuando se completa <see cref="LoadUserOperation"/>. 
    ''' Utilice este controlador de eventos para pasar de la "IU de carga" que creó en <see cref="InitializeRootVisual"/> a la "IU de la aplicación".
    ''' </summary>
    Private Sub Application_UserLoaded(ByVal operation As LoadUserOperation)
        If operation.HasError Then
            ErrorWindow.CreateNew(operation.Error)
            operation.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' Inicializa la propiedad <see cref="Application.RootVisual"/>. 
    ''' La IU inicial se mostrará antes de que se haya completado la operación LoadUser.
    ''' La operación LoadUser hará que el usuario se registre automáticamente si utiliza autenticación de Windows o si ha seleccionado la opción "Mantener la sesión iniciada" en un inicio de sesión anterior.
    ''' </summary>
    Protected Overridable Sub InitializeRootVisual()
        Dim busyIndicator As New $safeprojectname$.Controls.BusyIndicator()
        busyIndicator.Content = New MainPage()
        busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch
        busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch

        Me.RootVisual = busyIndicator
    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)
        ' Si la aplicación se está ejecutando fuera del depurador, notifique la excepción mediante un control ChildWindow.
        If Not System.Diagnostics.Debugger.IsAttached Then
            ' NOTA: esto permitirá que la aplicación se siga ejecutando después de que se haya iniciado, pero no controlado, una excepción. 
            ' En las aplicaciones de producción, este control de errores debería sustituirse por algo que notifique el error al sitio web y detenga la aplicación.
            e.Handled = True
            ErrorWindow.CreateNew(e.ExceptionObject)
        End If
    End Sub
End Class