Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Startseite der Anwendung.
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' Erstellt eine neue Instanz von <see cref="Home"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' Wird ausgeführt, wenn der Benutzer auf diese Seite navigiert.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class