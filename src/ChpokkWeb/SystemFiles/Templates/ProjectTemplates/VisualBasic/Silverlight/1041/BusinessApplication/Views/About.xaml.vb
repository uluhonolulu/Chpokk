Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' 現在のアプリケーションに関する情報を表す <see cref="Page"/> クラスです。
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' <see cref="About"/> クラスの新しいインスタンスを作成します。
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' ユーザーがこのページに移動するときに実行します。
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class