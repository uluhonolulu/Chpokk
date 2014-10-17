Imports System.Threading.Tasks
Imports Microsoft.AspNet.Mvc.Facebook.Models
Imports Microsoft.AspNet.Mvc.Facebook.Realtime

' To learn more about Facebook Realtime Updates, go to http://go.microsoft.com/fwlink/?LinkId=273887

Public Class UserRealtimeUpdateController
    Inherits FacebookRealtimeUpdateController

    Private ReadOnly Shared UserVerifyToken As String = ConfigurationManager.AppSettings("Facebook:VerifyToken:User")

    Public Overrides ReadOnly Property VerifyToken As String
        Get
            Return UserVerifyToken
        End Get
    End Property

    Public Overrides Function HandleUpdateAsync(notification As ChangeNotification) As Task 
        If notification.Object = "user" Then
            For Each entry In notification.Entry
                ' Your logic to handle the update here
            Next
        End If

        Throw New NotImplementedException()
    End Function
End Class