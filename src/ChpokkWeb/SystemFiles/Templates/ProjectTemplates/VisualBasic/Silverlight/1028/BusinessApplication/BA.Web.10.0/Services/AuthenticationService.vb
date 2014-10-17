Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: 部署此應用程式時切換到安全端點。
    ''       使用者的名稱和密碼只可以使用 https 傳遞。
    ''       若要執行此動作，請將 EnableClientAccessAttribute 上的 RequiresSecureEndpoint 屬性設定為 true。
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    ''       可以在 MSDN 上取得搭配  https 使用網域服務的詳細資訊。

    ''' <summary>
    ''' 網域服務負責在使用者登入應用程式時對使用者進行驗證。
    '''
    ''' 大部分的功能已由 AuthenticationBase 類別提供。
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace