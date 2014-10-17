<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of $safeprojectname$.ChangePasswordModel)" %>

<asp:Content ID="changePasswordTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Change Password
</asp:Content>

<asp:Content ID="changePasswordHeader" ContentPlaceHolderID="Header" runat="server">
    <%: Html.ActionLink("Cancel", "Index", "Home", Nothing, New With {.data_icon = "arrow-l", .data_rel = "back" }) %>
    <h1>Change Password</h1>
</asp:Content>

<asp:Content ID="changePasswordContent" ContentPlaceHolderID="MainContent" runat="server">
    <p>
        Passwords must be at least <%: Membership.MinRequiredPasswordLength %> characters long.
    </p>

    <% Using Html.BeginForm() %>
        <%: Html.AntiForgeryToken() %>
        <%: Html.ValidationSummary() %>

        <ul data-role="listview" data-inset="true">
            <li data-role="list-divider">Account information</li>

            <li data-role="fieldcontain">
                <%: Html.LabelFor(Function(m) m.OldPassword) %>
                <%: Html.PasswordFor(Function(m) m.OldPassword) %>                
            </li>

            <li data-role="fieldcontain">
                <%: Html.LabelFor(Function(m) m.NewPassword) %>
                <%: Html.PasswordFor(Function(m) m.NewPassword) %>                
            </li>

            <li data-role="fieldcontain">
                <%: Html.LabelFor(Function(m) m.ConfirmPassword) %>
                <%: Html.PasswordFor(Function(m) m.ConfirmPassword) %>                
            </li>

            <li data-role="fieldcontain">
                <input type="submit" value="Change password" />
            </li>
        </ul>
    <% End Using %>
</asp:Content>

<asp:Content ID="scriptsContent" ContentPlaceHolderID="ScriptsSection" runat="server">
    <%: Scripts.Render("~/bundles/jqueryval") %>
</asp:Content>
