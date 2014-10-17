Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "將 DisplayName 設成可繫結"

        ''' <summary>
        ''' 覆寫會於 <see cref="User.DisplayName"/> 變更時產生屬性變更通知的 <c>OnPropertyChanged</c> 方法。
        ''' </summary>
        ''' <param name="e">屬性變更事件引數。</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace