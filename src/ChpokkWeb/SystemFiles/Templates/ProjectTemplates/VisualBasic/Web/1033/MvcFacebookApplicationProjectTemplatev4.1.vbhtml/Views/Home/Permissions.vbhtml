@Imports Microsoft.AspNet.Mvc.Facebook
@ModelType FacebookRedirectContext
@Code
    ViewData("Title") = "Required Permissions"
End Code

@If Model.RequiredPermissions.Length > 0
    @<h3>You need to grant the following permission(s) on Facebook to view this page:</h3>
    @<ul>
    @For Each permission in Model.RequiredPermissions
        @<li>@permission</li>
    Next
    </ul>
    @<a class="buttonLink" href="@Html.Raw(Model.RedirectUrl)" target="_top">Authorize this application</a>
End If