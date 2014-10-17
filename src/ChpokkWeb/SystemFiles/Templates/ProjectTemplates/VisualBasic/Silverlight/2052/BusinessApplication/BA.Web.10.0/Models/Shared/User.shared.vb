Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' 返回用户显示名称，默认情况下为用户的 FriendlyName。
        ''' 如果未设置 FriendlyName，则返回 UserName。
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