Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports $safeprojectname$.LoginUI

''' <summary>
''' Classe <see cref="UserControl"/> fournissant l'interface utilisateur de l'application.
''' </summary>
Partial Public Class MainPage
    Inherits UserControl

    ''' <summary>
    ''' Crée une nouvelle instance <see cref="MainPage"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Une fois que le Frame navigue, assurez-vous que le contrôle <see cref="HyperlinkButton"/> représentant la page actuelle est sélectionné
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
    ''' Si une erreur se produit au cours de la navigation, afficher une fenêtre d'erreurs
    ''' </summary>
    Private Sub ContentFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        e.Handled = True
        ErrorWindow.CreateNew(e.Exception)
    End Sub
End Class