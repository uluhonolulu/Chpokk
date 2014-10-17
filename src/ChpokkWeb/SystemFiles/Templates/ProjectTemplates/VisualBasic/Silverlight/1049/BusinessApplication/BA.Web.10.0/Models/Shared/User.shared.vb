Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' Возвращает отображаемое имя пользователя, которое по умолчанию равно значению свойства FriendlyName.
        ''' Если свойство FriendlyName не задано, возвращается имя пользователя.
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