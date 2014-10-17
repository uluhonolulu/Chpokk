Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "Macht DisplayName bindbar"

        ''' <summary>
        ''' Überschreiben der Methode <c>OnPropertyChanged</c>, die Benachrichtigungen für die Änderung der Eigenschaft erzeugt, wenn sich <see cref="User.DisplayName"/> ändert.
        ''' </summary>
        ''' <param name="e">Die Ereignisargumente für die Änderung der Eigenschaft.</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace