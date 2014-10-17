@Code
    ' Set the layout page and page title
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Registration Confirmation Page"

    Dim message As String = ""
    Dim confirmationToken = Request("confirmationCode")

    WebSecurity.Logout()
    If Not confirmationToken.IsEmpty() Then
        If WebSecurity.ConfirmAccount(confirmationToken) Then
            message = "Registration Confirmed! Click on the Login tab to log in to the site."
        Else
            message = "Could not confirm your registration info"
        End If
    End If
End Code

@If Not message.IsEmpty() Then
    @<p>@message</p>
Else
    @<form method="post" action="">
        <fieldset>
            <legend>Confirmation Code</legend>
            <label for="confirmationCode">
                Please enter the confirmation code sent to you via email and 
                then click the <em>Confirm</em> button.
            </label>
            <input type="text" id="confirmationCode" name="confirmationCode" title="Confirmation code" />
            <input type="submit" value="Confirm" title="Confirm registration" />
        </fieldset>
    </form>
End If