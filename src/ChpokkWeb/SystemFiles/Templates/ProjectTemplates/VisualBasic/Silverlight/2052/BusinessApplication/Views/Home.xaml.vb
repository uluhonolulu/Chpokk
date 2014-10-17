Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' 应用程序的主页。
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' 创建新 <see cref="Home"/> 实例。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' 当用户导航到此页面时执行。
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class