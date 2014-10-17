@ModelType $safeprojectname$.LocalPasswordModel

<h3>Change password</h3>

@using Html.BeginForm("Manage", "Account", FormMethod.Post, New With {.id = "changePasswordForm"})
    @Html.AntiForgeryToken()
    @Html.ValidationSummary()

    @<fieldset>
        <legend>Change Password Form</legend>
        <ol>
            <li>
                @Html.LabelFor(Function(m) m.OldPassword)
                @Html.PasswordFor(Function(m) m.OldPassword)
            </li>
            <li>
                @Html.LabelFor(Function(m) m.NewPassword)
                @Html.PasswordFor(Function(m) m.NewPassword)
            </li>
            <li>
                @Html.LabelFor(Function(m) m.ConfirmPassword)
                @Html.PasswordFor(Function(m) m.ConfirmPassword)
            </li>
        </ol>
        <input type="submit" value="Change password" />
    </fieldset>
End Using
