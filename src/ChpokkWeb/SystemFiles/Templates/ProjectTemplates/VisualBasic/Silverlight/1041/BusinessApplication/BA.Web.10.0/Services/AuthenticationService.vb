Imports System.Security.Authentication
Imports System.ServiceModel.DomainServices.Hosting
Imports System.ServiceModel.DomainServices.Server
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Imports System.Threading
Namespace Web

    '' TODO: アプリケーションを配置するときに安全なエンドポイントに切り替えます。
    ''       ユーザーの名前とパスワードは、https のみを使用して渡す必要があります。
    ''       そのためには、EnableClientAccessAttribute で RequiresSecureEndpoint プロパティを true に設定します。
    ''   
    ''       <EnableClientAccess(RequiresSecureEndpoint:=True)>
    ''
    ''       https とドメイン サービスの併用に関する詳細については、MSDN を参照してください。

    ''' <summary>
    ''' ユーザーがアプリケーションにログオンするときに、ドメイン サービスはそのユーザーの認証を行います。
    '''
    ''' ほとんどの機能は、AuthenticationBase クラスによって既に提供されています。
    ''' </summary>
    <EnableClientAccess()> _
    Public Class AuthenticationService
        Inherits AuthenticationBase(Of User)
    End Class
End Namespace