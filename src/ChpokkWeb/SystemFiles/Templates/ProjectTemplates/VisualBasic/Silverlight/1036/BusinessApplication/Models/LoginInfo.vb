Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace LoginUI
    ''' <summary>
    ''' L'entité interne est utilisée pour faciliter la liaison entre les contrôles d'interface utilisateur (DataForm et l'étiquette affichant une erreur de validation) et les informations d'identification de connexion entrées par l'utilisateur.
    ''' </summary>
    Public Class LoginInfo
        Inherits ComplexObject
        Private _UserName As String

        ''' <summary>
        ''' Obtient et définit le nom d'utilisateur.
        ''' </summary>
        <Display(Name:="UserNameLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <Required()> _
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                If (String.Equals(Me._UserName, value) = False) Then
                    Me.ValidateProperty("UserName", value)
                    Me._UserName = value
                    Me.RaisePropertyChanged("UserName")
                End If
            End Set
        End Property

        ''' <summary>
        ''' Obtient ou définit une fonction qui retourne le mot de passe.
        ''' </summary>
        Friend Property PasswordAccessor As Func(Of String)

        ''' <summary>
        ''' Obtient et définit le mot de passe.
        ''' </summary>
        <Display(Name:="PasswordLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <Required()> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get
            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)

                ' Ne pas stocker le mot de passe dans un champ privé car il ne doit pas être stocké en mémoire en texte brut.
                ' À la place, le PasswordAccessor fourni sert de magasin de stockage pour la valeur.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        Private _RememberMe As Boolean

        ''' <summary>
        ''' Obtient et définit la valeur indiquant si les informations d'authentification de l'utilisateur doivent être enregistrées pour des connexions ultérieures.
        ''' </summary>
        <Display(Name:="RememberMeLabel", ResourceType:=GetType(ApplicationStrings))> _
        Public Property RememberMe() As Boolean
            Get
                Return _RememberMe
            End Get
            Set(ByVal value As Boolean)
                If _RememberMe <> value Then
                    Me.ValidateProperty("RememberMe", value)
                    Me._RememberMe = value
                    Me.RaisePropertyChanged("RememberMe")
                End If
            End Set
        End Property

        Private _CurrentLoginOperation As LoginOperation

        ''' <summary>
        ''' Obtient ou définit l'opération de connexion en cours.
        ''' </summary>
        Friend Property CurrentLoginOperation() As LoginOperation
            Get
                Return _CurrentLoginOperation
            End Get
            Set(ByVal value As LoginOperation)
                If Not Object.Equals(_CurrentLoginOperation, value) Then
                    If Not IsNothing(_CurrentLoginOperation) Then
                        RemoveHandler _CurrentLoginOperation.Completed, AddressOf Me.CurrentLoginOperationChanged
                    End If

                    _CurrentLoginOperation = value

                    If Not IsNothing(_CurrentLoginOperation) Then
                        AddHandler _CurrentLoginOperation.Completed, AddressOf Me.CurrentLoginOperationChanged
                    End If

                    Me.CurrentLoginOperationChanged(Me, EventArgs.Empty)
                End If
            End Set
        End Property

        ''' <summary>
        ''' Obtient une valeur indiquant si l'utilisateur est actuellement en cours de connexion.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property IsLoggingIn() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentLoginOperation)) AndAlso (Not Me.CurrentLoginOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Obtient une valeur indiquant si l'utilisateur peut actuellement se connecter.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property CanLogIn As Boolean
            Get
                Return (Not Me.IsLoggingIn)
            End Get
        End Property

        ''' <summary>
        ''' Déclenche des notifications de modification de propriété liée à l'opération lorsque l'opération de connexion en cours change.
        ''' </summary>
        Private Sub CurrentLoginOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsLoggingIn")
            Me.RaisePropertyChanged("CanLogIn")
        End Sub

        ''' <summary>
        ''' Crée une nouvelle instance <see cref="LoginParameters"/> à l'aide des données stockées dans cette entité.
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, Me.RememberMe, Nothing)
        End Function
    End Class
End Namespace