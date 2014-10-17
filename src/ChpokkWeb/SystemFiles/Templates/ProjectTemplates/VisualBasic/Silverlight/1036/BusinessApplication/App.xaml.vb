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
    ''' Crée une nouvelle instance <see cref="App"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        ' Créer un WebContext et l'ajouter à la collection ApplicationLifetimeObjects.
        ' Ce sera alors disponible en tant que WebContext.Current.
        Dim webContext As New WebContext()
        webContext.Authentication = New FormsAuthentication()
        'webContext.Authentication = New WindowsAuthentication()
        Me.ApplicationLifetimeObjects.Add(webContext)
    End Sub

    Private Sub Application_Startup(ByVal sender As Object, ByVal e As StartupEventArgs)
        ' Cela vous permet de lier des contrôles dans des fichiers XAML à des propriétés WebContext.Current
        Me.Resources.Add("WebContext", WebContext.Current)

        ' Cela authentifie automatiquement un utilisateur lorsque l'authentification Windows est utilisée ou lorsque l'utilisateur a choisi « Maintenir la connexion » lors d'une précédente tentative de connexion.
        WebContext.Current.Authentication.LoadUser(AddressOf Me.Application_UserLoaded, Nothing)

        ' Afficher certains éléments d'interface utilisateur à l'utilisateur pendant que LoadUser est en cours
        Me.InitializeRootVisual()
    End Sub

    ''' <summary>
    ''' Appelé lorsque le <see cref="LoadUserOperation"/> se termine. 
    ''' Utilisez ce gestionnaire d'événements pour basculer de l'« interface utilisateur de chargement » que vous avez créée dans <see cref="InitializeRootVisual"/> vers l'« interface utilisateur de l'application ».
    ''' </summary>
    Private Sub Application_UserLoaded(ByVal operation As LoadUserOperation)
        If operation.HasError Then
            ErrorWindow.CreateNew(operation.Error)
            operation.MarkErrorAsHandled()
        End If
    End Sub

    ''' <summary>
    ''' Initialise la propriété <see cref="Application.RootVisual"/>. 
    ''' L'interface utilisateur initiale s'affiche avant que l'opération LoadUser soit terminée.
    ''' L'opération LoadUser entraîne la connexion automatique de l'utilisateur si l'authentification Windows est utilisée ou si l'utilisateur a sélectionné l'option « Maintenir la connexion » lors d'une précédente connexion.
    ''' </summary>
    Protected Overridable Sub InitializeRootVisual()
        Dim busyIndicator As New $safeprojectname$.Controls.BusyIndicator()
        busyIndicator.Content = New MainPage()
        busyIndicator.HorizontalContentAlignment = HorizontalAlignment.Stretch
        busyIndicator.VerticalContentAlignment = VerticalAlignment.Stretch

        Me.RootVisual = busyIndicator
    End Sub

    Private Sub Application_UnhandledException(ByVal sender As Object, ByVal e As ApplicationUnhandledExceptionEventArgs)
        ' Si l'application s'exécute en dehors du débogueur, signaler l'exception à l'aide d'un contrôle ChildWindow.
        If Not System.Diagnostics.Debugger.IsAttached Then
            ' REMARQUE : cela permet à l'application de continuer à s'exécuter après qu'une exception a été levée mais pas gérée. 
            ' Pour des applications de production, cette gestion des erreurs doit être remplacée par un système qui signale l'erreur au site Web et arrête l'application.
            e.Handled = True
            ErrorWindow.CreateNew(e.ExceptionObject)
        End If
    End Sub
End Class