Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' Classe contenente le informazioni sull'utente autenticato.
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' NOTA: è possibile aggiungere le proprietà del profilo da utilizzare nell'applicazione Silverlight.
        ' Per abilitare i profili, modificare la sezione appropriata del file web.config.
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
        ''' Ottiene e imposta il nome descrittivo dell'utente.
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