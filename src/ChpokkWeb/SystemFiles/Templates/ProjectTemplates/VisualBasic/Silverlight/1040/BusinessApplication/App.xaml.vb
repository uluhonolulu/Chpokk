Imports System
Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls


''' <summary>
''' Classe <see cref="Application"/> principale.
''' </summary>
Partial Public Class App
    Inherits Application

    ''' <summary>
    ''' Crea una nuova istanza di <see cref="App"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        ' Creare un oggetto WebContext e aggiungerlo al set ApplicationLifetimeObjects.
        ' Tale oggetto sarà quindi disponibile come WebContext.Current.
        Dim webContext As New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        Me.ApplicationLifetimeObjects.Add(webContext)
    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
        ' Ciò consentirà di associare i controlli presenti nei file XAML alle proprietà WebContext.Current
        Me.Resources.Add("WebContext", WebContext.Current)

        ' In questo modo l'utente verrà autenticato automaticamente se viene utilizzata l'autenticazione di Windows o se è stata selezionata l'opzione "Mantieni l'accesso" durante un tentativo di accesso precedente.
        WebContext.Current.Authentication.LoadUser(AddressOf Me.Application_UserLoaded, Nothing)

        ' Mostrare all'utente una parte dell'interfaccia utente durante l'operazione LoadUser
        Me.InitializeRootVisual()
    End Sub

    ''' <summary>
    ''' Richiamato al completamento di <see cref="LoadUserOperation"/>. 
    ''' Utilizzare questo gestore eventi per passare dall'interfaccia di caricamento creata in <see cref="InitializeRootVisual"/> all'interfaccia utente dell'applicazione.
    ''' </summary>
    Private Sub Application_UserLoaded(ByVal operation As LoadUserOperation)
        If operation.HasError Then
            ErrorWindow.CreateNew(operation.Error)
            operation.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' Inizializza la proprietà <see cref="Application.RootVisual"/>. 
    ''' L'interaccia utente iniziale verrà visualizzata prima del completamento dell'operazione LoadUser.
    ''' L'operazione LoadUser comporterà l'accesso automatico dell'utente se viene utilizzata l'autenticazione di Windows o se è stata selezionata l'opzione "Mantieni l'accesso" durante un accesso precedente.
    ''' </summary>
    Protected Overridable Sub InitializeRootVisual()
        Dim busyIndicator As New $safeprojectname$.Controls.BusyIndicator()
        busyIndicator.Content = New MainPage()
        busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch
        busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch

        Me.RootVisual = busyIndicator
    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)
        ' Se l'applicazione viene eseguita all'esterno del debugger, segnalare l'eccezione mediante un controllo ChildWindow.
        If Not System.Diagnostics.Debugger.IsAttached Then
            ' NOTA: in questo modo sarà possibile continuare l'esecuzione dell'applicazione dopo la generazione di un'eccezione, anche se non gestita. 
            ' Per le applicazioni di produzione è consigliabile invece segnalare l'errore al sito Web e arrestare l'applicazione.
            e.Handled = True
            ErrorWindow.CreateNew(e.ExceptionObject)
        End If
    End Sub
End Class