Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "Make DisplayName Bindable"

        ''' <summary>
        ''' Override of the <c>OnPropertyChanged</c> method that generates property change notifications when <see cref="User.DisplayName"/> changes.
        ''' </summary>
        ''' <param name="e">The property change event args.</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace