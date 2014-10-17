Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Home page for the application.
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' Creates a new <see cref="Home"/> instance.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' Executes when the user navigates to this page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class