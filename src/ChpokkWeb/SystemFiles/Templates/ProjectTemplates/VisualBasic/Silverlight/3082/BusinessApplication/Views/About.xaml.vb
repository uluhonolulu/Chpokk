Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Clase <see cref="Page"/> para presentar información sobre la aplicación actual.
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' Crea una nueva instancia de la clase <see cref="About"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' Se ejecuta cuando el usuario navega a esta página.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class