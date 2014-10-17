Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' 應用程式的首頁。
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' 建立新 <see cref="Home"/> 執行個體。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' 於使用者巡覽到此頁面時執行。
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class