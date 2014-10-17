Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' 包含有关已通过身份验证的用户的信息的类。
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' 注意: 可以添加配置文件属性以在 Silverlight 应用程序中使用。
        ' 若要启用配置文件，请编辑 web.config 文件的相应部分。
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
        ''' 获取和设置用户的友好名称。
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