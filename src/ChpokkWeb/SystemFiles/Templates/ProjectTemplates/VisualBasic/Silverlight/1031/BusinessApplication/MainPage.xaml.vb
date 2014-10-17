Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports $safeprojectname$.LoginUI

''' <summary>
''' <see cref="UserControl"/> Klasse, die die Haupt-Benutzeroberfläche für die Anwendung bereitstellt.
''' </summary>
Partial Public Class MainPage
    Inherits UserControl

    ''' <summary>
    ''' Erstellt eine neue Instanz von <see cref="MainPage"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Nach der Navigation durch den Frame sicherstellen, dass der <see cref="HyperlinkButton"/> ausgewählt ist, der die aktuelle Seite darstellt
    ''' </summary>
    Private Sub ContentFrame_Navigated(ByVal sender As Object, ByVal e As NavigationEventArgs)
        For Each child As UIElement In LinksStackPanel.Children
            Dim hb As HyperlinkButton = TryCast(child, HyperlinkButton)
            If hb IsNot Nothing AndAlso hb.NavigateUri IsNot Nothing Then
                If hb.NavigateUri.ToString().Equals(e.Uri.ToString()) Then
                    VisualStateManager.GoToState(hb, "ActiveLink", True)
                Else
                    VisualStateManager.GoToState(hb, "InactiveLink", True)
                End If
            End If
        Next
    End Sub

    ''' <summary>
    ''' Ein Fehlerfenster anzeigen, wenn während der Navigation ein Fehler auftritt
    ''' </summary>
    Private Sub ContentFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        e.Handled = True
        ErrorWindow.CreateNew(e.Exception)
    End Sub
End Class