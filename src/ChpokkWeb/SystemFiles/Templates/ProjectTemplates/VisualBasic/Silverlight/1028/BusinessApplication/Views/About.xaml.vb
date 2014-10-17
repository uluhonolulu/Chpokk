Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' 呈現目前應用程式相關資訊的 <see cref="Page"/> 類別。
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' 建立 <see cref="About"/> 類別的新執行個體。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' 於使用者巡覽到此頁面時執行。
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class