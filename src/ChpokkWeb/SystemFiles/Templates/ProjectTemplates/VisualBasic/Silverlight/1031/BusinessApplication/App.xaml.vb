Imports System
Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls


''' <summary>
''' Haupt <see cref="Application"/>-Klasse.
''' </summary>
Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' Erstellt eine neue Instanz von <see cref="App"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        ' Einen WebContext erstellen und der Sammlung ApplicationLifetimeObjects hinzufügen.
        ' Dieser ist anschließend als WebContext.Current verfügbar.
        Dim webContext As New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        Me.ApplicationLifetimeObjects.Add(webContext)
    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
        ' Damit können Steuerelemente in XAML-Dateien an Eigenschaften von WebContext.Current gebunden werden
        Me.Resources.Add("WebContext", WebContext.Current)

        ' Dadurch wird ein Benutzer automatisch authentifiziert, wenn er die Windows-Authentifizierung benutzt oder er bei einer früheren Anmeldung die Option "Angemeldet bleiben" ausgewählt hat.
        WebContext.Current.Authentication.LoadUser(AddressOf Me.Application_UserLoaded, Nothing)

        ' Dem Benutzer die Benutzeroberfläche anzeigen, während LoadUser ausgeführt wird
        Me.InitializeRootVisual()
    End Sub

    ''' <summary>
    ''' Wird beim Abschluss von <see cref="LoadUserOperation"/> aufgerufen. 
    ''' Nutzen Sie diesen Ereignishandler, um von der "Ladebenutzeroberfläche", die in <see cref="InitializeRootVisual"/> erstellt wurde, auf die "Anwendungsbenutzeroberfläche" umzuschalten.
    ''' </summary>
    Private Sub Application_UserLoaded(ByVal operation As LoadUserOperation)
        If operation.HasError Then
            ErrorWindow.CreateNew(operation.Error)
            operation.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' Initialisiert die Eigenschaft <see cref="Application.RootVisual"/>. 
    ''' Die ursprüngliche Benutzeroberfläche wird angezeigt, bevor der LoadUser-Vorgang abgeschlossen ist.
    ''' Durch den LoadUser-Vorgang wird ein Benutzer automatisch angemeldet, wenn er die Windows-Authentifizierung benutzt oder er bei einer früheren Anmeldung die Option "Angemeldet bleiben" ausgewählt hat.
    ''' </summary>
    Protected Overridable Sub InitializeRootVisual()
        Dim busyIndicator As New $safeprojectname$.Controls.BusyIndicator()
        busyIndicator.Content = New MainPage()
        busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch
        busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch

        Me.RootVisual = busyIndicator
    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)
        ' Falls die Anwendung außerhalb des Debuggers ausgeführt wird, melden Sie die Ausnahme mithilfe eines ChildWindow-Steuerelements.
        If Not System.Diagnostics.Debugger.IsAttached Then
            ' HINWEIS: So kann die Anwendung weiterhin ausgeführt werden, nachdem eine Ausnahme ausgelöst, aber nicht behandelt wurde. 
            ' Bei Produktionsanwendungen sollte diese Fehlerbehandlung durch eine Anwendung ersetzt werden, die den Fehler der Website meldet und die Anwendung beendet.
            e.Handled = True
            ErrorWindow.CreateNew(e.ExceptionObject)
        End If
    End Sub
End Class