Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Navigation
Imports $safeprojectname$.LoginUI

''' <summary>
''' 为应用程序提供主 UI 的 <see cref="UserControl"/> 类。
''' </summary>
Partial Public Class MainPage
    Inherits UserControl

    ''' <summary>
    ''' 创建新 <see cref="MainPage"/> 实例。
    ''' </summary>
    Public Sub New()
        InitializeComponent()
    End Sub

    ''' <summary>
    ''' 在 Frame 导航之后，请确保选中表示当前页的 <see cref="HyperlinkButton"/>
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
    ''' 如果导航过程中出现错误，则显示错误窗口
    ''' </summary>
    Private Sub ContentFrame_NavigationFailed(ByVal sender As Object, ByVal e As NavigationFailedEventArgs)
        e.Handled = True
        ErrorWindow.CreateNew(e.Exception)
    End Sub
End Class