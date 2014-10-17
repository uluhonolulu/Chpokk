Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' 응용 프로그램의 홈 페이지입니다.
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' 새 <see cref="Home"/> 인스턴스를 만듭니다.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' 사용자가 이 페이지를 탐색할 때 실행됩니다.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class