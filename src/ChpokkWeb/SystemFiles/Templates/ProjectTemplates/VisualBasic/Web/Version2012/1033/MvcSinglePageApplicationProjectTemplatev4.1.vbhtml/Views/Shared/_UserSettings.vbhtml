@If Request.IsAuthenticated
    @<text>
        Hello, @Html.ActionLink(User.Identity.Name, "Manage", "Account", routeValues:=New With {.area = ""}, htmlAttributes:=New With {.class = "username", .title = "Manage"})
        @Using Html.BeginForm("LogOff", "Account", New With {.area = ""}, FormMethod.Post, New With {.id = "logoutForm"})
            @Html.AntiForgeryToken()
            @<a href="javascript:document.getElementById('logoutForm').submit()">Log off</a>
        End Using
    </text>
End If