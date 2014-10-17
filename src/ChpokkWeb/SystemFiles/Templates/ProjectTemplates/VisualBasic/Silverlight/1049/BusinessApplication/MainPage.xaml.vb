Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports $safeprojectname$.LoginUI

''' <summary>
''' Класс <see cref="UserControl"/> реализует основную функциональность пользовательского интерфейса приложения.
''' </summary>
Partial Public Class MainPage
    Inherits UserControl

    ''' <summary>
    ''' Создает новый экземпляр класса <see cref="MainPage"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' После перехода в рамке (Frame) проверьте, что ссылка <see cref="HyperlinkButton"/> представляет текущую выбранную страницу
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
    ''' Если при переходе возникла ошибка, отобразить окно сообщения
    ''' </summary>
    Private Sub ContentFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        e.Handled = True
        ErrorWindow.CreateNew(e.Exception)
    End Sub
End Class