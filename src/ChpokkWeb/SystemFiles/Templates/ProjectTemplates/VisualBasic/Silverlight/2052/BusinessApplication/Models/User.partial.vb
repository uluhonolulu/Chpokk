Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "使 DisplayName 可绑定"

        ''' <summary>
        ''' 用于在 <see cref="User.DisplayName"/> 更改时生成属性更改通知的 <c>OnPropertyChanged</c> 方法重写。
        ''' </summary>
        ''' <param name="e">属性更改事件参数。</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace