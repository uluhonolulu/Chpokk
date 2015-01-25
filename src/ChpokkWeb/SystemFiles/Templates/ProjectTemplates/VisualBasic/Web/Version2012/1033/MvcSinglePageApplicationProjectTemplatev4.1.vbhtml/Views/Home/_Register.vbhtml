@ModelType $safeprojectname$.RegisterModel

@Using Html.BeginForm("JsonRegister", "Account", New With { .ReturnUrl = ViewData("ReturnUrl") }, FormMethod.Post, New With {.Id = "registerForm"})
    @Html.AntiForgeryToken()

    @<fieldset>
        <legend>Registration Form</legend>
        <ol>
            <li>
                @Html.LabelFor(Function(m) m.UserName)
                @Html.TextBoxFor(Function(m) m.UserName, New With {.id = "registerName"})
            </li>
            <li>
                @Html.LabelFor(Function(m) m.Password)
                @Html.PasswordFor(Function(m) m.Password)
            </li>
            <li>
                @Html.LabelFor(Function(m) m.ConfirmPassword)
                @Html.PasswordFor(Function(m) m.ConfirmPassword)
            </li>
            <li>
                <input type="submit" value="Sign up" />
            </li>
        </ol>
        @Html.ValidationSummary()
    </fieldset>
End Using