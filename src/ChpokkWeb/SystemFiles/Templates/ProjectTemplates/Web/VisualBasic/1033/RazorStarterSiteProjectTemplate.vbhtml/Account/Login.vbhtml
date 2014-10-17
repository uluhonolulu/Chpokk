@Code
    ' Set the layout page and page title
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Log In"

    ' Initialize general page variables
    Dim email = ""
    Dim password = ""
    Dim rememberMe = False

    ' Validation
    Dim isValid = True
    Dim emailErrorMessage = ""
    Dim passwordErrorMessage = ""

    ' If this is a POST request, validate and process data
    If IsPost Then
        email = Request.Form("email")
        password = Request.Form("password")
        rememberMe = Request.Form("rememberMe").AsBool()

        ' Validate the user's email
        If email.IsEmpty() Then
            emailErrorMessage = "You must specify an email address."
            isValid = False
        End If

        ' Validate the user's password
        If password.IsEmpty() Then
            passwordErrorMessage = "You must specify a password."
            isValid = False
        End If

        ' Confirm there are no validation errors
        If isValid Then
            If WebSecurity.UserExists(email) AndAlso WebSecurity.GetPasswordFailuresSinceLastSuccess(email) > 4 AndAlso WebSecurity.GetLastPasswordFailureDate(email).AddSeconds(60) > Date.UtcNow Then
                Response.Redirect("~/account/AccountLockedOut")
                Return
            End If

            ' Attempt to login to the Security object using provided creds
            If WebSecurity.Login(email, password, rememberMe) Then
                Dim returnUrl = Request.QueryString("ReturnUrl")
                If returnUrl.IsEmpty() Then
                    Response.Redirect("~/")
                Else
                    Response.Redirect(returnUrl)
                End If
            End If

            ' If we arrived here, the login failed; convey that to the user
            isValid = False
        End If
    End If
End Code

<p>
   Please enter your Email address and password below. If you don't have an account,
   visit the <a href="@Href("~/Account/Register")">registration page</a> and create one.
</p>

@*If one or more validation errors exist, show an error*@
@If Not isValid Then
   @<p class="message error">There was a problem with your login and/or errors exist in your form.</p>
End If

<form method="post" action="">
    <fieldset>
        <legend>Log In to Your Account</legend>
        <ol>
            <li class="email">
                <label for="email">Email address:</label>
                <input type="text" id="email" name="email" value="@email" title="email" @IIf((Not emailErrorMessage.IsEmpty()), "class=""error-field", Nothing)/>
                @*Write any email validation errors to the page*@
                @If Not emailErrorMessage.IsEmpty() Then
                @<label for="email" class="validation-error">
                    @emailErrorMessage
                </label>
                End If
            </li>
            <li class="password">
                <label for="password">Password:</label>
                <input type="password" id="password" name="password" title="Password" @IIf((Not passwordErrorMessage.IsEmpty()), "class=""error-field", Nothing)/>
                @*Write any password validation errors to the page*@
                @If Not passwordErrorMessage.IsEmpty() Then
                @<label for="password" class="validation-error">
                    @passwordErrorMessage
                </label>
                End If
            </li>
            <li class="remember-me">
                <label class="checkbox" for="rememberMe">Remember Me?</label>
                <input type="checkbox" id="rememberMe" name="rememberMe" value="true" title="Remember Me" @IIf((rememberMe), "checked=""checked", Nothing)/>
            </li>
        </ol>
        <p class="form-actions">
            <input type="submit" value="login" title="Login"/>
        </p>
        <p><a href="@Href("~/Account/ForgotPassword")">Did you forget your password?</a></p>
    </fieldset>
</form>