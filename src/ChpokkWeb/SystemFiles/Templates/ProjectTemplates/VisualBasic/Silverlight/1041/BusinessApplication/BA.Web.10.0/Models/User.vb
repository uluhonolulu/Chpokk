Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' 認証されているユーザーに関する情報を含むクラスです。
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' メモ: Silverlight アプリケーションで使用するためにプロファイル プロパティを追加できます。
        ' プロファイルを有効にするには、web.config ファイルの該当する部分を編集してください。
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
        ''' ユーザーのフレンドリ名を取得および設定します。
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