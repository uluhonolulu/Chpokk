Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' 현재 응용 프로그램에 대한 정보를 제공하기 위한 <see cref="Page"/> 클래스입니다.
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' <see cref="About"/> 클래스의 새 인스턴스를 만듭니다.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' 사용자가 이 페이지를 탐색할 때 실행됩니다.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class