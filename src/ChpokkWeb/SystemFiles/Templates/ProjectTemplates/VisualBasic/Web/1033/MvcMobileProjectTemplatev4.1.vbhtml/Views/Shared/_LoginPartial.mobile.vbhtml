@If Request.IsAuthenticated Then
    @Html.ActionLink(User.Identity.Name, "Index", "Account", routeValues:=Nothing, htmlAttributes:=New With {.data_icon = "gear"})
Else
    @Html.ActionLink("Log in", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {.data_icon = "gear"})
End If
