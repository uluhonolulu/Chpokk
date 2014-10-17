Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' 用于提供有关当前应用程序的信息的 <see cref="Page"/> 类。
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' 创建 <see cref="About"/> 类的新实例。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' 当用户导航到此页面时执行。
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class