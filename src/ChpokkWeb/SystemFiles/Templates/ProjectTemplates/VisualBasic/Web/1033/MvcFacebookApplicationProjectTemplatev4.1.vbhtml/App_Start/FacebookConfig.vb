Imports Microsoft.AspNet.Mvc.Facebook
Imports Microsoft.AspNet.Mvc.Facebook.Authorization

Public Module FacebookConfig
    Public Sub Register(ByVal configuration As FacebookConfiguration)
        ' Loads the settings from web.config using the following app setting keys:
        ' Facebook:AppId, Facebook:AppSecret, Facebook:AppNamespace
        configuration.LoadFromAppSettings()

        ' Adding the authorization filter to check for Facebook signed requests and permissions
        GlobalFilters.Filters.Add(New FacebookAuthorizeFilter(configuration))
    End Sub
End Module