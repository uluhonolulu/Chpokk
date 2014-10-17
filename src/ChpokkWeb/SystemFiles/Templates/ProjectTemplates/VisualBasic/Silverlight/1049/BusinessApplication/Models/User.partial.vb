Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "Делает DisplayName привязываемым"

        ''' <summary>
        ''' Переопределение метода <c>OnPropertyChanged</c>, который вызывает уведомления об изменении свойства при изменении <see cref="User.DisplayName"/>.
        ''' </summary>
        ''' <param name="e">Аргументы события изменения свойств.</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace