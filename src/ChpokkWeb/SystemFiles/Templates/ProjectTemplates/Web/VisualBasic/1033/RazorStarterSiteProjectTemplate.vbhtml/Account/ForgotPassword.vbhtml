@Code
    ' Set the layout page and page title
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Forget Your Password?"

    Dim passwordSent As Boolean = False
    Dim isValid As Boolean = True
    Dim errorMessage = ""
    Dim emailError = ""
    Dim disabledAttribute = ""
    Dim resetToken = ""
    Dim email = If(Request.Form("email"), Request.QueryString("email"))

    If IsPost Then
        ' validate email
        If email.IsEmpty() OrElse (Not email.Contains("@")) Then
            emailError = "Please enter a valid email"
            isValid = False
        End If
        If isValid Then
            If WebSecurity.GetUserId(email) > -1 AndAlso WebSecurity.IsConfirmed(email) Then
                resetToken = WebSecurity.GeneratePasswordResetToken(email) 'Optionally specify an expiration date for the token
            Else
                passwordSent = True ' We don't want to disclose that the user does not exist.
                isValid = False
            End If
        End If
        If isValid Then
            Dim hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)
            Dim resetUrl = hostUrl + VirtualPathUtility.ToAbsolute("~/Account/PasswordReset?resetToken=" & HttpUtility.UrlEncode(resetToken))
            WebMail.Send(to:= email, subject:= "Please reset your password", body:= "Use this password reset token to reset your password. The token is: " & resetToken & ". Visit <a href=""" & resetUrl & """>" & resetUrl & "</a> to reset your password.")
            passwordSent = True
            disabledAttribute = "disabled=""disabled"""
        End If
    End If
End Code

<form method="post" action="">
    <fieldset>
        <legend>Password Reset Instructions Form</legend>
        @If Not WebMail.SmtpServer.IsEmpty() Then
            @<p>
                We will send password reset instructions to the email address associated with your account. 
            </p>
            If passwordSent Then
                @<p class="message success">
                    Instructions to reset your password have been sent to the specified email address.
                </p>
            End If
            If Not errorMessage.IsEmpty() Then
                @<p class="message error">
                    @errorMessage
                </p>
            End If
            @<ol>
                <li class="email">
                    <label for="email">Email Address</label>
                    <input type="text" id="email" name="email" title="Email address" value="@email" @disabledAttribute @IIf((Not emailError.IsEmpty()), "class=""error-field",Nothing)/>
                    @If Not emailError.IsEmpty() Then
                        @<label class="validation-error">@emailError</label>
                    End If
                </li>
            </ol>
            @<p class="form-actions">
                <input type="submit" value="Send Instructions" @disabledAttribute/>
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