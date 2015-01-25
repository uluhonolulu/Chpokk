<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<% if (Request.IsAuthenticated) { %>
    <%: Html.ActionLink(Page.User.Identity.Name, "Index", "Account", routeValues: null, htmlAttributes: new { data_icon = "gear" }) %>
<% } else { %>
    <%: Html.ActionLink("Log in", "Login", "Account", routeValues: null, htmlAttributes: new { data_icon = "gear" }) %>
<% } %>
