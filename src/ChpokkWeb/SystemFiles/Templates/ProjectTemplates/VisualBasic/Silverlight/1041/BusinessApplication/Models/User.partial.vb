Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "DisplayName をバインド可能にします"

        ''' <summary>
        ''' <see cref="User.DisplayName"/> が変更された場合にプロパティの変更通知を生成する <c>OnPropertyChanged</c> メソッドのオーバーライドです。
        ''' </summary>
        ''' <param name="e">プロパティ変更イベントの引数です。</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace