Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' <see cref="Page"/> class to present information about the current application.
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' Creates a new instance of the <see cref="About"/> class.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' Executes when the user navigates to this page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class