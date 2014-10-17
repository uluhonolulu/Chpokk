﻿<!DOCTYPE html>
<html>
<head>$if$ ($usehtml5$ == True)
    <meta charset="utf-8" />$endif$
    <title>@ViewData("Title")</title>
    <link href="@Url.Content("~/Content/Site.css")" rel="stylesheet" type="text/css" />
    <script src="@Url.Content("~/Scripts/jquery-1.7.1.min.js")" type="text/javascript"></script>$if$ ($usehtml5$ == True)
    <script src="@Url.Content("~/Scripts/modernizr-2.5.3.js")" type="text/javascript"></script>$endif$
</head>

<body>
    @RenderBody()
</body>
</html>
