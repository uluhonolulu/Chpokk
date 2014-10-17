<%@ Page Language="VB" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage(Of $safeprojectname$.LoginModel)" %>

<asp:Content runat="server" ID="indexTitle" ContentPlaceHolderID="TitleContent">
    Your Account
</asp:Content>

<asp:Content runat="server" ID="indexHeader" ContentPlaceHolderID="Header">
    <%: Html.ActionLink("Back", "Index", "Home", Nothing, New With {.data_icon = "arrow-l", .data_rel = "back" }) %>
    <h1>Your Account</h1>
</asp:Content>

<asp:Content runat="server" ID="indexContent" ContentPlaceHolderID="MainContent">
    <p>
        Logged in as <strong><%: Page.User.Identity.Name %></strong>.
    </p>

    <ul data-role="listview" data-inset="true">
        <li><%: Html.ActionLink("Change password", "ChangePassword") %></li>
        <li><%: Html.ActionLink("Log off", "LogOff") %></li>
    </ul>
</asp:Content>