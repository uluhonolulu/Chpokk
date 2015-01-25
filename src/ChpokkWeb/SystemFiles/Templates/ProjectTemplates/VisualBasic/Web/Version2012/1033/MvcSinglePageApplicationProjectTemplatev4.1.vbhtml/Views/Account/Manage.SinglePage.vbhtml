@ModelType $safeprojectname$.LocalPasswordModel
@Code
    ViewData("Title") = "Manage Account"
End Code

<header>
    <h1>@ViewData("Title")</h1>
</header>

<div id="main-content">
    <p class="message-success">@ViewData("StatusMessage")</p>

    <p>You're logged in as <strong>@User.Identity.Name</strong>.</p>
    <p>@Html.ActionLink("Back to My Todo List", "Index", "Home")</p>

    <section class="todoList">
    @If ViewData("HasLocalPassword") Then
        @Html.Partial("_ChangePasswordPartial")
    Else
        @Html.Partial("_SetPasswordPartial")
    End If
    </section>

    <section class="todoList" id="externalLogins">
        @Html.Action("RemoveExternalLogins")

        <h3>Add an external login</h3>
        @Html.Action("ExternalLoginsList", New With {.ReturnUrl = ViewData("ReturnUrl")})
    </section>
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
End Section
