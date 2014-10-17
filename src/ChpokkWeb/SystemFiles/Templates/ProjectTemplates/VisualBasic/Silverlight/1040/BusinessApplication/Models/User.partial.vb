Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "Rendi DisplayName associabile"

        ''' <summary>
        ''' Eseguire l'override del metodo <c>OnPropertyChanged</c> che genera notifiche di modifica delle proprietà in caso di modifica di <see cref="User.DisplayName"/>.
        ''' </summary>
        ''' <param name="e">Argomenti dell'evento di modifica delle proprietà.</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace