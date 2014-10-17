Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' Gibt den Benutzeranzeigenamen zurück, welcher standardmäßig dem FriendlyName entspricht.
        ''' Wenn FriendlyName nicht festgelegt ist, wird der Benutzername zurückgegeben.
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