Imports Newtonsoft.Json

Public Class FacebookPhoto
    ' This renames the property to picture.
    <JsonProperty("picture")> 
    Public Property ThumbnailUrl As String

    Public Property Link As String
End Class