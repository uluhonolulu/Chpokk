Imports System.Net.Http
Imports System.Web.Http.Controllers
Imports System.Net
Imports System.Web.Http.Filters
Imports System.Web.Helpers

Public Class ValidateHttpAntiForgeryTokenAttribute
    Inherits AuthorizationFilterAttribute

    Public Overrides Sub OnAuthorization(actionContext As HttpActionContext)
        Dim request As HttpRequestMessage = actionContext.ControllerContext.Request

        Try
            If IsAjaxRequest(request) Then
                ValidateRequestHeader(request)
            Else
                AntiForgery.Validate()
            End If
        Catch e As HttpAntiForgeryException
            actionContext.Response = request.CreateErrorResponse(HttpStatusCode.Forbidden, e)
        End Try
    End Sub

    Private Function IsAjaxRequest(request As HttpRequestMessage) As Boolean
        Dim xRequestedWithHeaders As IEnumerable(Of String) = Nothing

        If request.Headers.TryGetValues("X-Requested-With", xRequestedWithHeaders) Then
            Dim headerValue As String = xRequestedWithHeaders.FirstOrDefault()
            If Not String.IsNullOrEmpty(headerValue) Then
                Return String.Equals(headerValue, "XMLHttpRequest", StringComparison.OrdinalIgnoreCase)
            End If
        End If

        Return False
    End Function

    Private Sub ValidateRequestHeader(request As HttpRequestMessage)
        Dim cookieToken As String = String.Empty
        Dim formToken As String = String.Empty

        Dim tokenHeaders As IEnumerable(Of String) = Nothing
        If request.Headers.TryGetValues("RequestVerificationToken", tokenHeaders) Then
            Dim tokenValue As String = tokenHeaders.FirstOrDefault()
            If Not String.IsNullOrEmpty(tokenValue) Then
                Dim tokens As String() = tokenValue.Split(":"c)
                If tokens.Length = 2 Then
                    cookieToken = tokens(0).Trim()
                    formToken = tokens(1).Trim()
                End If
            End If
        End If

        AntiForgery.Validate(cookieToken, formToken)
    End Sub
End Class