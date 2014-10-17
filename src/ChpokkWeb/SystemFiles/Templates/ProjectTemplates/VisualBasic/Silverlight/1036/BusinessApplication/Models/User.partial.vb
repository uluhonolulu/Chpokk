Imports System
Imports System.ComponentModel

Namespace Web

    Partial Public Class User
#Region "Rendre DisplayName Bindable"

        ''' <summary>
        ''' Substitution de la méthode <c>OnPropertyChanged</c> qui génère des notifications de modification de propriété lorsque <see cref="User.DisplayName"/> change.
        ''' </summary>
        ''' <param name="e">Arguments de l'événement de modification de la propriété.</param>
        Protected Overrides Sub OnPropertyChanged(ByVal e As System.ComponentModel.PropertyChangedEventArgs)
            MyBase.OnPropertyChanged(e)

            If e.PropertyName = "Name" Or e.PropertyName = "FriendlyName" Then
                Me.RaisePropertyChanged("DisplayName")
            End If
        End Sub
#End Region
    End Class
End Namespace