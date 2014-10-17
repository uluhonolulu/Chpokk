Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' 包含已驗證使用者相關資訊的類別。
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' 注意: 可以加入設定檔屬性以便在 Silverlight 應用程式中使用。
        ' 若要啟用設定檔，請編輯 web.config 檔案的適當區段。
        '
        ' Private myProfile As String
        ' Public Property MyProfileProperty() As String
        '     Get
        '         Return myProfile
        '     End Get
        '     Set(ByVal value As String)
        '         myProfile = value
        '     End Set
        ' End Property

        Private _FriendlyName As String

        ''' <summary>
        ''' 取得和設定使用者的易記名稱。
        ''' </summary>
        Public Property FriendlyName() As String
            Get
                Return _FriendlyName
            End Get
            Set(ByVal value As String)
                _FriendlyName = value
            End Set
        End Property
    End Class
End Namespace