Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' Class containing information about the authenticated user.
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' NOTE: Profile properties can be added for use in Silverlight application.
        ' To enable profiles, edit the appropriate section of web.config file.
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
        ''' Gets and sets the friendly name of the user.
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