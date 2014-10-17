Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' 傳回使用者顯示名稱 (預設為它的 FriendlyName)，
        ''' 如果沒有設定 FriendlyName，則會傳回使用者名稱。
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