Imports System.Threading.Tasks
Imports Microsoft.AspNet.Mvc.Facebook
Imports Microsoft.AspNet.Mvc.Facebook.Client

Public Class HomeController
    Inherits Controller

    <FacebookAuthorize("email", "user_photos")>
    Public Async Function Index(context As FacebookContext) As Task(Of ActionResult)
        If ModelState.IsValid Then
            Dim user = await context.Client.GetCurrentUserAsync(Of MyAppUser)()
            Return View(user)
        End If

        Return View("Error")
    End Function

    ' This action will handle the redirects from FacebookAuthorizeFilter when 
    ' the app doesn't have all the required permissions specified in the FacebookAuthorizeAttribute.
    ' The path to this action is defined under appSettings (in Web.config) with the key 'Facebook:AuthorizationRedirectPath'.
    Public Function Permissions(context As FacebookRedirectContext) As ActionResult
        if ModelState.IsValid Then
            Return View(context)
        End If

        Return View("Error")
    End Function
End Class