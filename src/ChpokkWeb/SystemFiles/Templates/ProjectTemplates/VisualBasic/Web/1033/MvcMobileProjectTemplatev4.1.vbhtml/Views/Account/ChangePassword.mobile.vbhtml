@ModelType $safeprojectname$.ChangePasswordModel

@Code
    ViewData("Title") = "Change Password"
End Code

@section Header
    @Html.ActionLink("Cancel", "Index", "Home", Nothing, New With {.data_icon = "arrow-l", .data_rel = "back"})
    <h1>@ViewData("Title")</h1>
End Section

<p>
    Passwords must be at least @Membership.MinRequiredPasswordLength characters long.
</p>

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary()

    @<ul data-role="listview" data-inset="true">
        <li data-role="list-divider">Account information</li>

        <li data-role="fieldcontain">
            @Html.LabelFor(Function(m) m.OldPassword)
            @Html.PasswordFor(Function(m) m.OldPassword)
        </li>

        <li data-role="fieldcontain">
            @Html.LabelFor(Function(m) m.NewPassword)
            @Html.PasswordFor(Function(m) m.NewPassword)
        </li>

        <li data-role="fieldcontain">
            @Html.LabelFor(Function(m) m.ConfirmPassword)
            @Html.PasswordFor(Function(m) m.ConfirmPassword)
        </li>

        <li data-role="fieldcontain">
            <input type="submit" value="Change password" />
        </li>
    </ul>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
