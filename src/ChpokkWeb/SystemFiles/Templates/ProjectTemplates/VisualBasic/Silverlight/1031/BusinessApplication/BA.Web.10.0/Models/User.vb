Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' Klasse, die Informationen über den authentifizierten Benutzer enthält.
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' HINWEIS: Profileigenschaften können für die Verwendung in der Silverlight-Anwendung hinzugefügt werden.
        ' Um Profile zu aktivieren, bearbeiten Sie den entsprechenden Abschnitt der Datei "web.config".
        '
        ' Private myProfile As String
        ' Public Property MyProfileProperty() As String
        '     Get
        '         Return myProfile
        '     End Get
        '     Set(ByVal value As String)
        '         myProfile = value
        '     End Set
        ' End Property

        Private _FriendlyName As String

        ''' <summary>
        ''' Ruft den Anzeigenamen des Benutzers ab und legt ihn fest.
        ''' </summary>
        Public Property FriendlyName() As String
            Get
                Return _FriendlyName
            End Get
            Set(ByVal value As String)
                _FriendlyName = value
            End Set
        End Property
    End Class
End Namespace