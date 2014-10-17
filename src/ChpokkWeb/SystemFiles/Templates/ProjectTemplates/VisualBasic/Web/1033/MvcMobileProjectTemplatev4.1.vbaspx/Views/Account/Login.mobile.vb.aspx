<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of $safeprojectname$.LoginModel)" %>

<asp:Content ID="loginTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Log in
</asp:Content>

<asp:Content ID="loginHeader" ContentPlaceHolderID="Header" runat="server">
    <%: Html.ActionLink("Cancel", "Index", "Home", Nothing, New With {.data_icon = "arrow-l", .data_rel = "back" }) %>
    <h1>Log in</h1>
</asp:Content>

<asp:Content ID="loginContent" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Please enter your user name and password. <%: Html.ActionLink("Register", "Register") %> if you don't have an account.
    </p>

    <% Using Html.BeginForm() %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary() %>

        <ul data-role="listview" data-inset="true">
            <li data-role="list-divider">Details</li>

            <li data-role="fieldcontain">
                <%: Html.LabelFor(Function(m) m.UserName) %>
                <%: Html.TextBoxFor(Function(m) m.UserName) %>                
            </li>

            <li data-role="fieldcontain">
                <%: Html.LabelFor(Function(m) m.Password) %>
                <%: Html.PasswordFor(Function(m) m.Password) %>                
            </li>

            <li data-role="fieldcontain">
                <%: Html.LabelFor(Function(m) m.RememberMe) %>
                <%: Html.CheckBoxFor(Function(m) m.RememberMe) %>
            </li>

            <li data-role="fieldcontain">
                <input type="submit" value="Log in" />
            </li>
        </ul>
    <% End Using %>
</asp:Content>

<asp:Content ID="scriptsContent" ContentPlaceHolderID="ScriptsSection" runat="server">
    <%: Scripts.Render("~/bundles/jqueryval") %>
</asp:Content>
