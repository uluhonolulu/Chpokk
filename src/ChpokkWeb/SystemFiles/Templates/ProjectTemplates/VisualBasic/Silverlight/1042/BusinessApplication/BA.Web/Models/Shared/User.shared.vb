Namespace Web

    Partial Public Class User
        ''' <summary>
        ''' 사용자 표시 이름(기본값: FriendlyName)을 반환합니다.
        ''' FriendlyName을 설정하지 않은 경우 사용자 이름이 반환됩니다.
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