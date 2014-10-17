Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports $safeprojectname$.LoginUI

''' <summary>
''' 응용 프로그램에 대한 기본 UI를 제공하는 <see cref="UserControl"/> 클래스입니다.
''' </summary>
Partial Public Class MainPage
    Inherits UserControl

    ''' <summary>
    ''' 새 <see cref="MainPage"/> 인스턴스를 만듭니다.
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' Frame이 이동된 후 현재 페이지를 표시하는 <see cref="HyperlinkButton"/>이 선택되어 있는지 확인합니다.
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
    ''' 이동 중에 오류가 발생하는 경우 오류 창을 표시합니다.
    ''' </summary>
    Private Sub ContentFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        e.Handled = True
        ErrorWindow.CreateNew(e.Exception)
    End Sub
End Class