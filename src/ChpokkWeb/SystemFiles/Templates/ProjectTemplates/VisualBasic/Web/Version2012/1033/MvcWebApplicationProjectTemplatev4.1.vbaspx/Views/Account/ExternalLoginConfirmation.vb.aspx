<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of $safeprojectname$.RegisterExternalLoginModel)" %>

<asp:Content ID="externalLoginConfirmationTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Register with your <%: ViewData("ProviderDisplayName") %> account
</asp:Content>

<asp:Content ID="externalLoginConfirmationContent" ContentPlaceHolderID="MainContent" runat="server">
    <hgroup class="title">
        <h1>Register.</h1>
        <h2>Associate your <%: ViewBag.ProviderDisplayName %> account.</h2>
    </hgroup>

    <% Using Html.BeginForm("ExternalLoginConfirmation", "Account", New With { .ReturnUrl = ViewData("ReturnUrl") }) %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Association Form</legend>
            <p>
                You've successfully authenticated with <strong><%: ViewData("ProviderDisplayName") %></strong>.
                Please enter a user name for this site below and click the Confirm button to finish
                logging in.
            </p>
            <ol>
                <li class="name">
                    <%: Html.LabelFor(Function(m) m.UserName) %>
                    <%: Html.TextBoxFor(Function(m) m.UserName) %>
                    <%: Html.ValidationMessageFor(Function(m) m.UserName) %>
                </li>
            </ol>
            <%: Html.HiddenFor(Function(m) m.ExternalLoginData) %>
            <input type="submit" value="Register" />
        </fieldset>
    <% End Using %>
</asp:Content>

<asp:Content ID="scriptsContent" ContentPlaceHolderID="ScriptsSection" runat="server">
    <%: Scripts.Render("~/bundles/jqueryval") %>
</asp:Content>
