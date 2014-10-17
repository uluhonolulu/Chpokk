@Code
    ' Log out of the current user context
    WebSecurity.Logout()

    ' Redirect back to the homepage or return URL
    Dim returnUrl = Request.QueryString("ReturnUrl")
    If returnUrl.IsEmpty() Then
        Response.Redirect("~/")
    Else
        Response.Redirect(returnUrl)
    End If
End Code