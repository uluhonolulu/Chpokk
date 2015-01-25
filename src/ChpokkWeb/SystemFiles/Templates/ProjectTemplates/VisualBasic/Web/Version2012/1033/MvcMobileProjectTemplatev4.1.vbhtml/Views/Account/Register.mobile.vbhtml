@ModelType $safeprojectname$.RegisterModel

@Code
    ViewData("Title") = "Register"
End Code

<p>
    Passwords must be at least @Membership.MinRequiredPasswordLength characters long.
</p>

@Using Html.BeginForm()
    @Html.AntiForgeryToken()
    @Html.ValidationSummary()

    @<ul data-role="listview" data-inset="true">
        <li data-role="list-divider">Details</li>

        <li data-role="fieldcontain">
            @Html.LabelFor(Function(m) m.UserName)
            @Html.TextBoxFor(Function(m) m.UserName)
        </li>

        <li data-role="fieldcontain">
            @Html.LabelFor(Function(m) m.Email)
            @Html.TextBoxFor(Function(m) m.Email)            
        </li>

        <li data-role="fieldcontain">
            @Html.LabelFor(Function(m) m.Password)
            @Html.PasswordFor(Function(m) m.Password)            
        </li>

        <li data-role="fieldcontain">
            @Html.LabelFor(Function(m) m.ConfirmPassword)
            @Html.PasswordFor(Function(m) m.ConfirmPassword)            
        </li>

        <li data-role="fieldcontain">
            <input type="submit" value="Register" />
        </li>
    </ul>
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section