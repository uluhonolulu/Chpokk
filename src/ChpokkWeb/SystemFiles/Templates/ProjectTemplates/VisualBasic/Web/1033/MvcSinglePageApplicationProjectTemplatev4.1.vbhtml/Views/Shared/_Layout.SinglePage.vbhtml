﻿<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <title>@ViewData("Title") - My ASP.NET MVC Application</title>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    @Styles.Render("~/Content/css")
    @Scripts.Render("~/bundles/modernizr")
    @Styles.Render("~/Content/themes/base/css")
</head>
<body>
    <div id="userSettings">
        @Html.Partial("_UserSettings")
    </div>

    <div id="body">
        @RenderBody()
    </div>

    <footer>
        <nav>
            <ul id="menu">
                <li>@Html.ActionLink("Todo List", "Index", "Home", new With { .area = "" }, Nothing)</li>
                <li>@Html.ActionLink("API", "Index", "Help", new With { .area = "HelpPage" }, Nothing)</li>
            </ul>
        </nav> 
        <p>Learn more about <a href="http://go.microsoft.com/fwlink/?LinkId=273732">Single Page Applications</a></p>
        <p>&copy; @DateTime.Now.Year - My ASP.NET MVC Application</p>
    </footer>

    @Scripts.Render("~/bundles/jquery")
    @Scripts.Render("~/bundles/jqueryui")
    @Scripts.Render("~/bundles/ajaxlogin")
    @RenderSection("scripts", required:= false)
</body>
</html>