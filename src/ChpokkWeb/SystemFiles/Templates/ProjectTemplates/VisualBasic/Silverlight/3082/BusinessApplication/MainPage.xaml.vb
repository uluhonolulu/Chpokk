Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports $safeprojectname$.LoginUI

''' <summary>
''' Clase <see cref="UserControl"/> que proporciona la IU principal de la aplicación.
''' </summary>
Partial Public Class MainPage
    Inherits UserControl

    ''' <summary>
    ''' Crea una nueva instancia de <see cref="MainPage"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Una vez que la clase base Frame navegue, asegúrese de que esté seleccionado el <see cref="HyperlinkButton"/> que representa la página actual
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
    ''' Si se produce un error durante la navegación, mostrar una ventana de error
    ''' </summary>
    Private Sub ContentFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        e.Handled = True
        ErrorWindow.CreateNew(e.Exception)
    End Sub
End Class