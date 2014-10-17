@Code
    ' Set the layout page and page title
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Register an Account"

    ' Initialize general page variables
    Dim email = ""
    Dim password = ""
    Dim confirmPassword = ""

    ' Validation
    Dim isValid = True
    Dim emailErrorMessage = ""
    Dim passwordErrorMessage = ""
    Dim confirmPasswordMessage = ""
    Dim accountCreationErrorMessage = ""
    ' Dim captchaMessage = ""

    ' If this is a POST request, validate and process data
    If IsPost Then
        email = Request.Form("email")
        password = Request.Form("password")
        confirmPassword = Request.Form("confirmPassword")

        ' Validate the user's captcha answer
        ' If Not ReCaptcha.Validate("PRIVATE_KEY") Then
        '     captchaMessage = "Captcha response was not correct"
        '     isValid = False
        ' End If

        ' Validate the user's email address
        If email.IsEmpty() Then
            emailErrorMessage = "You must specify an email address."
            isValid = False
        End If

        ' Validate the user's password and password confirmation
        If password.IsEmpty() Then
            passwordErrorMessage = "The password cannot be blank."
            isValid = False
        End If

        If password <> confirmPassword Then
            confirmPasswordMessage = "The new password and confirmation password do not match."
            isValid = False
        End If

        ' If all information is valid, create a new account
        If isValid Then
            ' Insert a new user into the database
            Dim db = Database.Open("StarterSite")

            ' Check if user already exists
            Dim user = db.QuerySingle("SELECT Email FROM UserProfile WHERE LOWER(Email) = LOWER(@0)", email)
            If user Is Nothing Then
                ' Insert email into the profile table
                db.Execute("INSERT INTO UserProfile (Email) VALUES (@0)", email)
                ' Create and associate a new entry in the membership database.
                ' If successful, continue processing the request
                Try
                    Dim requireEmailConfirmation As Boolean = Not WebMail.SmtpServer.IsEmpty()
                    Dim token = WebSecurity.CreateAccount(email, password, requireEmailConfirmation)
                    If requireEmailConfirmation Then
                        Dim hostUrl = Request.Url.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped)
                        Dim confirmationUrl = hostUrl + VirtualPathUtility.ToAbsolute("~/Account/Confirm?confirmationCode=" & HttpUtility.UrlEncode(token))
                        WebMail.Send(to:= email, subject:= "Please confirm your account", body:= "Your confirmation code is: " & token & ". Visit <a href=""" & confirmationUrl & """>" & confirmationUrl & "</a> to activate your account.")
                    End If

                    If requireEmailConfirmation Then
                        ' Thank the user for registering and let them know an email is on its way
                        Response.Redirect("~/Account/Thanks")
                    Else
                        ' Navigate back to the homepage and exit
                        WebSecurity.Login(email, password)
                        Response.Redirect("~/")
                    End If
                Catch e As System.Web.Security.MembershipCreateUserException
                    isValid = False
                    accountCreationErrorMessage = e.ToString()
                End Try
            Else
                ' User already exists
                isValid = False
                accountCreationErrorMessage = "Email address is already in use."
            End If
        End If
    End If
End Code

<p>
   Use the form below to create a new account. 
</p>

@*If at least one validation error exists, notify the user*@
@If Not isValid Then
   @<p class="message error">
    @If accountCreationErrorMessage.IsEmpty() Then
        @:Please correct the errors and try again.
    Else
        @accountCreationErrorMessage
    End If
   </p>
End If

<form method="post" action="">
    <fieldset>
        <legend>Sign-up Form</legend>
        <ol>
            <li class="email">
                <label for="email">Email:</label>
                <input type="text" id="email" name="email" title="Email address" value="@email" @IIf((Not emailErrorMessage.IsEmpty()), "class=""error-field", Nothing)/>
                @*Write any email validation errors to the page*@
                @If Not emailErrorMessage.IsEmpty() Then
                    @<label for="email" class="validation-error">@emailErrorMessage</label>
                End If
            </li>
            <li class="password">
                <label for="password">Password:</label>
                <input type="password" id="password" name="password" title="Password" @IIf((Not passwordErrorMessage.IsEmpty()), "class=""error-field", Nothing)/>
                @*Write any password validation errors to the page*@
                @If Not passwordErrorMessage.IsEmpty() Then
                    @<label for="password" class="validation-error">@passwordErrorMessage</label>
                End If
            </li>
            <li class="confirm-password">
                <label for="confirmPassword">Confirm Password:</label>
                <input type="password" id="confirmPassword" name="confirmPassword" title="Confirm password" @IIf((Not confirmPasswordMessage.IsEmpty()), "class=""error-field", Nothing)/>
                @*Write any password validation errors to the page*@
                @If Not confirmPasswordMessage.IsEmpty() Then
                    @<label for="confirmPassword" class="validation-error">@confirmPasswordMessage</label>
                End If
            </li>
            <li class="recaptcha">
                <div class="message info">
                    <p>To enable CAPTCHA verification, <a href="http://go.microsoft.com/fwlink/?LinkId=204140">install the ASP.NET Web Helpers Library</a> and uncomment ReCaptcha.Render and replace 'PUBLIC_KEY'
                    with your public key.  At the top of this page, uncomment ReCaptcha.Validate and
                    replace 'PRIVATE_KEY' with your private key, and also uncomment the captchaMessage variable.</p>
                    <p>Register for reCAPTCHA keys at <a href="http://recaptcha.net">reCAPTCHA.net</a>.</p>
                </div>
                @*
                @ReCaptcha.GetHtml("PUBLIC_KEY", theme: "white")
                @If Not captchaMessage.IsEmpty() Then
                    <label class="validation-error">@captchaMessage</label>
                End If
                *@
            </li>
        </ol>
        <p class="form-actions">
            <input type="submit" value="Register" title="Register" />
        </p>
    </fieldset>
</form>