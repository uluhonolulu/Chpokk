Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' Classe contenant des informations sur l'utilisateur authentifié.
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' REMARQUE : des propriétés de profil peuvent être ajoutées pour être utilisées dans l'application Silverlight.
        ' Pour activer les profils, modifiez la section appropriée du fichier web.config.
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
        ''' Obtient et définit le nom convivial de l'utilisateur.
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