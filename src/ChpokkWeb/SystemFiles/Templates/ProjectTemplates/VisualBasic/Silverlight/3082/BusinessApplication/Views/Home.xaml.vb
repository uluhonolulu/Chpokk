Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Página de inicio de la aplicación.
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' Crea una nueva instancia de <see cref="Home"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario navega a esta page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class