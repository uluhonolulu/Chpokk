@Code
    ' Set the layout page and page title
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Password Reset"

    Dim newPasswordError = ""
    Dim confirmPasswordError = ""
    Dim passwordResetTokenError = ""
    Dim passwordResetToken = If(Request.Form("resetToken"), Request.QueryString("resetToken"))
    Dim disabledAttribute = ""

    Dim isValid As Boolean = True
    Dim tokenExpired As Boolean = False
    Dim isSuccess As Boolean = False

    If IsPost Then
        Dim newPassword = Request("newPassword")
        Dim confirmPassword = Request("confirmPassword")

        If newPassword.IsEmpty() Then
            newPasswordError = "Please enter a new password."
            isValid = False
        End If

        If confirmPassword <> newPassword Then
            confirmPasswordError = "The password confirmation did not match the new password."
            isValid = False
        End If

        If passwordResetToken.IsEmpty() Then
            passwordResetTokenError = "Please enter your password reset token. It should have been sent to you in an email."
            isValid = False
        End If

        If isValid Then
            If WebSecurity.ResetPassword(passwordResetToken, newPassword) Then
                disabledAttribute = "disabled=""disabled"""
                isSuccess = True
            Else
                passwordResetTokenError = "The password reset token is invalid."
                tokenExpired = True
                isValid = False
            End If
        Else
            isValid = False
        End If
    End If
End Code

@If Not isValid Then
    @<p class="message error">
        @If tokenExpired Then
            @<text>The password reset token is incorrect or may be expired. Visit the <a href="@Href("~/Account/ForgotPassword")">forgot password page</a> to generate a new one.</text>
        Else
            @<text>Could not reset password. Please correct the errors and try again.</text>
        End If
    </p>
End If

@If isSuccess Then
    @<p class="message success">
        Password changed! Click <a href="@Href("~/Account/Login")" title="Login">here</a> to login.
    </p>
End If

<form method="post" action="">
    <fieldset>
        <legend>Password Change Form</legend>
        @If Not WebMail.SmtpServer.IsEmpty() Then
            @<p>Please type in your new password below and click the <em>Reset Password</em> button to change your password.</p>
            @<ol>
                <li class="new-password">
                    <label for="newPassword">New Password:</label> 
                    <input type="password" id="newPassword" name="newPassword" title="New Password" @disabledAttribute @IIf((Not newPasswordError.IsEmpty()), "class=""error-field", Nothing) />
                    @If Not newPasswordError.IsEmpty() Then
                        @<label for="newPassword" class="validation-error">@newPasswordError</label>
                    End If
                </li>
                <li class="confirm-password">
                    <label for="confirmPassword">Confirm Password:</label> 
                    <input type="password" id="confirmPassword" name="confirmPassword" title="Confirm new password" @disabledAttribute @IIf((Not confirmPasswordError.IsEmpty()), "class=""error-field", Nothing)/>
                    @If Not confirmPasswordError.IsEmpty() Then
                        @<label for="confirmPassword" class="validation-error">@confirmPasswordError</label>
                    End If
                </li>
                <li class="reset-toekn">
                    <label for="resetToken">Password Reset Token:</label> 
                    <input type="text" id="resetToken" name="resetToken" value="@passwordResetToken" title="Password reset token" @disabledAttribute @IIf((Not passwordResetTokenError.IsEmpty()), "class=""error-field" ,Nothing)/>
                    @If Not passwordResetTokenError.IsEmpty() Then
                        @<label for="resetToken" class="validation-error">@passwordResetTokenError</label>
                    End If
                </li>
            </ol>
            @<p class="form-actions">
                <input type="submit" value="Reset Password" title="Reset password" @disabledAttribute/>
            </p>
        Else
            @<p class="message info">
                Password recovery is disabled for this website because the SMTP server is 
                not configured correctly. Please contact the owner of this site to reset 
                your password.
            </p>
        End If
    </fieldset>
</form>