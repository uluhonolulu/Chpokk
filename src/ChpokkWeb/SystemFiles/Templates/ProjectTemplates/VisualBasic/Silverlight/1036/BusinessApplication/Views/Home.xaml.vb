Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Page d'accueil de l'application.
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' Crée une nouvelle instance <see cref="Home"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' S'exécute lorsque l'utilisateur navigue vers cette page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class