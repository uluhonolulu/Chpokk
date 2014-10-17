Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: 在部署应用程序时切换为安全终结点。
    ''       只应使用 https 传递用户名和密码。
    ''       为此，请将 EnableClientAccessAttribute 的 RequiresSecureEndpoint 属性设置为 true。
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    ''       有关对域服务使用 https 的更多信息可在 MSDN 上找到。

    ''' <summary>
    ''' 负责在用户登录应用程序时对用户进行身份验证的域服务。
    '''
    ''' AuthenticationBase 类已提供了大部分功能。
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace