Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' Devuelve el nombre para mostrar del usuario, que de forma predeterminada es su FriendlyName.
        ''' Si no se ha establecido FriendlyName, se devuelve el nombre de usuario.
        ''' </summary>
        Public ReadOnly Property DisplayName() As String
            Get
                If Not String.IsNullOrEmpty(Me.FriendlyName) Then
                    Return Me.FriendlyName
                Else
                    Return Me.Name
                End If
            End Get
        End Property
    End Class
End Namespace