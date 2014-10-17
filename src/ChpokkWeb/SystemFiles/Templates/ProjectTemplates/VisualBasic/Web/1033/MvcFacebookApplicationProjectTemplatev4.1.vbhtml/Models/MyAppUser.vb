Imports Microsoft.AspNet.Mvc.Facebook
Imports Newtonsoft.Json

' Add any fields you want to be saved for each user and specify the field name in the JSON coming back from Facebook
' http://go.microsoft.com/fwlink/?LinkId=273889

Public Class MyAppUser
    Public Property Id As String
    Public Property Name As String
    Public Property Email As String

    ' This renames the property to picture and sets the picture size to large
    <JsonProperty("picture")> _
    <FacebookFieldModifier("type(large)")>
    Public Property ProfilePicture As FacebookConnection(Of FacebookPicture)

    ' This sets the size of the friend list to 8, remove it to get all friends.
    <FacebookFieldModifier("limit(8)")>
    Public Property Friends As FacebookGroupConnection(Of MyAppUserFriend)
    
    ' This sets the size of the photo list to 16, remove it to get all photos.
    <FacebookFieldModifier("limit(16)")>
    Public Property Photos As FacebookGroupConnection(Of FacebookPhoto)
End Class