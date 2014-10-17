Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Classe <see cref="Page"/> permettant de présenter les informations relatives à l'application actuelle.
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' Crée une nouvelle instance de la classe <see cref="About"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' S'exécute lorsque l'utilisateur navigue vers cette page.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class