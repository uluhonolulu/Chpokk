<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="contactTitle" ContentPlaceHolderID="TitleContent" runat="server">
    Contact
</asp:Content>

<asp:Content ID="contactContent" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Contact</h2>

    <h3>Phone</h3>
    <span>425.555.0100</span>

    <h3>Email</h3>
    <span><a href="mailto:General@example.com">General@example.com</a></span>

    <ul data-role="listview" data-inset="true">
        <li data-role="list-divider">Navigation</li>
        <li><%: Html.ActionLink("Home", "Index", "Home") %></li>
        <li><%: Html.ActionLink("About", "About", "Home") %></li>
    </ul>
</asp:Content>