Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' Restituisce il nome utente visualizzato, che per impostazione predefinita corrisponde all'oggetto FriendlyName.
        ''' Se FriendlyName non è impostato, viene restituito l'oggetto User Name.
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