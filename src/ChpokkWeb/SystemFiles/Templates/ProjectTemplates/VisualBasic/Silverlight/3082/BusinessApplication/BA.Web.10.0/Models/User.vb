Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' Clase que contiene información sobre el usuario autenticado.
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' NOTA: se pueden agregar propiedades de perfil para utilizarlas en la aplicación de Silverlight.
        ' Para habilitar perfiles, edite la sección correspondiente del archivo web.config.
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
        ''' Obtiene y establece el nombre descriptivo del usuario.
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