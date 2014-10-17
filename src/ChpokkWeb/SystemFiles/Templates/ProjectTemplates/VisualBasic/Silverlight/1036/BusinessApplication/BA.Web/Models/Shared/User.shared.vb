Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' Retourne le nom complet de l'utilisateur, qui est par défaut son FriendlyName.
        ''' Si FriendlyName n'est pas défini, le nom d'utilisateur est retourné.
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