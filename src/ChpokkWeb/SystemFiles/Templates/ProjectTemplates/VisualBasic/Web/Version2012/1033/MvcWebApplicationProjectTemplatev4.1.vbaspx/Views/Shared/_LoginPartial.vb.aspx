<%@ Page Language="VB" Inherits="System.Web.Mvc.ViewPage" %>
<% If (Request.IsAuthenticated) Then %>
    Hello, <%: Html.ActionLink(User.Identity.Name, "Manage", "Account", routeValues:=Nothing, htmlAttributes:=New With {.class = "username", .title = "Manage" }) %>!
    <% Using Html.BeginForm("LogOff", "Account", FormMethod.Post, New With { .Id = "logoutForm" }) %>
        <%: Html.AntiForgeryToken() %>
        <a href="javascript:document.getElementById('logoutForm').submit()">Log off</a>
    <% End Using %>
<% Else %>
    <ul>
        <li><%: Html.ActionLink("Register", "Register", "Account", routeValues:=Nothing, htmlAttributes:=New With {.id = "registerLink" }) %></li>
        <li><%: Html.ActionLink("Log in", "Login", "Account", routeValues:=Nothing, htmlAttributes:=New With {.id = "loginLink" }) %></li>
    </ul>
<% End If %>