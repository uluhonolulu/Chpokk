Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' Returns the user display name, which by default is its FriendlyName.
        ''' If FriendlyName is not set, the User Name is returned.
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