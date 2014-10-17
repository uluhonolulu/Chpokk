Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' ユーザーの表示名 (既定ではユーザーの FriendlyName) を返します。
        ''' FriendlyName が設定されていない場合は、ユーザー名が返されます。
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