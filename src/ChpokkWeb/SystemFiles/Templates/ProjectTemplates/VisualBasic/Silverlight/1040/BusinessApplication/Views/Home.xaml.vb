Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Home page dell'applicazione.
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' Crea una nuova istanza di <see cref="Home"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' Viene eseguito quando l'utente passa a questa pagina.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class