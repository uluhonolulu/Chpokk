@ModelType $safeprojectname$.LoginModel

@Code
    ViewData("Title") = "Your Account"
End Code

@section Header
    @Html.ActionLink("Back", "Index", "Home", Nothing, New With {.data_icon = "arrow-l", .data_rel = "back"})
    <h1>@ViewData("Title")</h1>
End Section

<p>
    Logged in as <strong>@User.Identity.Name</strong>.
</p>

<ul data-role="listview" data-inset="true">
    <li>@Html.ActionLink("Change password", "ChangePassword")</li>
    <li>@Html.ActionLink("Log off", "LogOff")</li>
</ul>