@ModelType $safeprojectname$.LoginModel

@Using Html.BeginForm("JsonLogin", "Account", New With { .ReturnUrl = ViewData("ReturnUrl") }, FormMethod.Post, New With { .id = "loginForm" })
    @Html.AntiForgeryToken()

    @<fieldset>
        <legend>Log in Form</legend>
        <ol>
            <li>
                @Html.LabelFor(Function(m) m.UserName)
                @Html.TextBoxFor(Function(m) m.UserName, New With {.autofocus = "autofocus", .id = "loginName"})
                @Html.ValidationMessageFor(Function(m) m.UserName)
            </li>
            <li>
                @Html.LabelFor(Function(m) m.Password)
                @Html.PasswordFor(Function(m) m.Password)
                @Html.ValidationMessageFor(Function(m) m.Password)
            </li>
            <li>
                @Html.CheckBoxFor(Function(m) m.RememberMe)
                @Html.LabelFor(Function(m) m.RememberMe, New With {.class = "checkbox"})
            </li>
            <li>
                <input type="submit" value="Log in" />
            </li>
        </ol>
        @Html.ValidationSummary() 
    </fieldset>
End Using