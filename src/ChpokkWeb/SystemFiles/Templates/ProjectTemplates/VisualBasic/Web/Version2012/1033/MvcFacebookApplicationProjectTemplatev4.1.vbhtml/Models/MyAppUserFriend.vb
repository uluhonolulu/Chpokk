Imports Microsoft.AspNet.Mvc.Facebook

' Add any fields you want to be saved for each user and specify the field name in the JSON coming back from Facebook
' http://go.microsoft.com/fwlink/?LinkId=273889

Public Class MyAppUserFriend
    Public Property Name As String

    Public Property Link As String

    ' This sets the picture height and width to 100px.
    <FacebookFieldModifier("height(100).width(100)")>
    Public Property Picture As FacebookConnection(Of FacebookPicture)
End Class