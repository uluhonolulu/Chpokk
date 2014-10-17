Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' <see cref="Page"/> Klasse zur Bereitstellung von Informationen über die aktuelle Anwendung.
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' Erstellt eine neue Instanz der <see cref="About"/>-Klasse.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' Wird ausgeführt, wenn der Benutzer auf diese Seite navigiert.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class