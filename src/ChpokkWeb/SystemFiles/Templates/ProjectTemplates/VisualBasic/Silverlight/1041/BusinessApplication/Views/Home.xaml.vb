Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' アプリケーションのホーム ページです。
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' 新しい <see cref="Home"/> インスタンスを作成します。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' ユーザーがこのページに移動するときに実行します。
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class