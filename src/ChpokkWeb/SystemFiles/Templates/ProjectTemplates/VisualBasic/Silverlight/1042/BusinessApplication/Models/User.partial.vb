Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "DisplayName을 바인딩 가능으로 지정"

        ''' <summary>
        ''' <see cref="User.DisplayName"/>이 변경될 때 속성 변경 알림을 생성하는 <c>OnPropertyChanged</c> 메서드의 재정의입니다.
        ''' </summary>
        ''' <param name="e">속성 변경 이벤트 인수입니다.</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace