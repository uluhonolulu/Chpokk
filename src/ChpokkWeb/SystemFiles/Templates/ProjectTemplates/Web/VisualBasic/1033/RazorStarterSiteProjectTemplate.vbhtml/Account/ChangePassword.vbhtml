@Code
    WebSecurity.RequireAuthenticatedUser()

    ' Set the layout page and page title
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Change Password"

    Dim isValid As Boolean = True
    Dim isSuccess As Boolean = False
    Dim errorMessage = ""
    Dim currentPasswordError = ""
    Dim newPasswordError = ""
    Dim confirmPasswordError = ""
    Dim currentPassword = Request("currentPassword")
    Dim newPassword = Request("newPassword")
    Dim confirmPassword = Request("confirmPassword")

    If IsPost Then
        If currentPassword.IsEmpty() Then
            currentPasswordError = "Please enter your current password."
            isValid = False
        End If
        If newPassword.IsEmpty() Then
            newPasswordError = "Please enter a new password."
            isValid = False
        End If
        If confirmPassword.IsEmpty() Then
            confirmPasswordError = "Please confirm your new password."
            isValid = False
        End If
        If confirmPassword <> newPassword Then
            confirmPasswordError = "The password confirmation does not match the new password."
            isValid = False
        End If

        If isValid Then
            If WebSecurity.ChangePassword(WebSecurity.CurrentUserName, currentPassword, newPassword) Then
                isSuccess = True
            Else
                errorMessage = "An error occurred when attempting to change the password. Please contact the site owner."
            End If
        Else
            errorMessage = "Password change failed. Please correct the errors and try again."
        End If
    End If
End Code

<form method="post" action="">
    <fieldset>
        <legend>Change Password Form</legend>
        <p>
            Use this form to change your password. You'll be required to enter your current password. 
            Click <a href="@Href("~/Account/ForgotPassword")" title="Forgot password page">here</a> if you've forgotten your password.
        </p>
        @If isSuccess Then
            @<p class="message success">
                Your password has been updated!
            </p>
        End If
        @If Not errorMessage.IsEmpty() Then
            @<p class="message error">
                @errorMessage
            </p>
        End If
        <ol>
            <li class="current-password">
                <label for="currentPassword">Current Password:</label>
                <input type="password" id="currentPassword" name="currentPassword" title="Current password" @IIf((Not currentPasswordError.IsEmpty()), "class=""error-field", Nothing)/>
                @If Not currentPasswordError.IsEmpty() Then
                    @<label for="currentPassword" class="validation-error">@currentPasswordError</label>
                End If
            </li>
            <li class="new-password">
                <label for="newPassword">New Password:</label> 
                <input type="password" id="newPassword" name="newPassword" title="New password" @IIf((Not newPasswordError.IsEmpty()),"class=""error-field", Nothing)/>
                @If Not newPasswordError.IsEmpty() Then
                    @<label for="newPassword" class="validation-error">@newPasswordError</label>
                End If
            </li>
            <li class="confirm-password">
                <label for="confirmPassword">Confirm Password:</label> 
                <input type="password" id="confirmPassword" name="confirmPassword" title="Confirm new password" @IIf((Not confirmPasswordError.IsEmpty()),"class=""error-field",Nothing)/>
                @If Not confirmPasswordError.IsEmpty() Then
                    @<label for="confirmPassword" class="validation-error">@confirmPasswordError</label>
                End If
            </li>
        </ol>
        <p class="form-actions">
            <input type="submit" value="Change Password" title="Change password" />
        </p>
    </fieldset>
</form>