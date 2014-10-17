Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "Convertir DisplayName en enlazable"

        ''' <summary>
        ''' Invalidación del método <c>OnPropertyChanged</c> que genera notificaciones de cambio de propiedades cuando cambia <see cref="User.DisplayName"/>.
        ''' </summary>
        ''' <param name="e">Argumentos del evento de cambio de la propiedad</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace