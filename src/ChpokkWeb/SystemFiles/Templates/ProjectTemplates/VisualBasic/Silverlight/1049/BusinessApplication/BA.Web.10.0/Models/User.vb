Imports System.Runtime.Serialization
Imports System.ServiceModel.DomainServices.Server.ApplicationServices
Namespace Web

    ''' <summary>
    ''' Класс содержит сведения о пользователе, прошедшем проверку подлинности.
    ''' </summary>
    Partial Public Class User
        Inherits UserBase
        ' ПРИМЕЧАНИЕ. Можно добавить свойства профиля для использования в приложении Silverlight.
        ' Чтобы включить профили, измените соответствующий раздел файла web.config.
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
        ''' Возвращает и задает понятное имя пользователя.
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