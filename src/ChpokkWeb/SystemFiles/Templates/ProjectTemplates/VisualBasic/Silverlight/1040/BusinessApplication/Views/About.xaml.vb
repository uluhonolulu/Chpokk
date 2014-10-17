Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Classe <see cref="Page"/> per mostrare informazioni sull'applicazione corrente.
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' Crea una nuova istanza della classe <see cref="About"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' Viene eseguito quando l'utente passa a questa pagina.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class