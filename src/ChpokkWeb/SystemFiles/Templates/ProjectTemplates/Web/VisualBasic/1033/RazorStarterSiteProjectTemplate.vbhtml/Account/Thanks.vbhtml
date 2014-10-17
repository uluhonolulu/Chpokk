@Code
    ' Set the layout page and page title
    Layout = "~/_SiteLayout.vbhtml"
    PageData("Title") = "Thanks for registering"
End Code

@If Not WebSecurity.IsAuthenticated Then
    @<h2>But you&#8217;re not done yet!</h2>
    @<p>
       An email with instructions on how to activate your account is on its way 
       to you.
    </p>
Else
    @<h2>You are all set.</h2>
    @<p>
        It looks like you&#8217;ve already confirmed your account and are 
        good to go.
    </p>
End If