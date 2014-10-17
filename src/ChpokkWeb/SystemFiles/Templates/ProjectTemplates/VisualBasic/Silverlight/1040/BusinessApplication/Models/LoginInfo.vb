Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace LoginUI
    ''' <summary>
    ''' Questa entità interna viene utilizzata per facilitare l'associazione tra i controlli dell'interfaccia utente (oggetto DataForm ed etichetta in cui viene visualizzato un errore di convalida) e le credenziali di accesso immesse dall'utente.
    ''' </summary>
    Public Class LoginInfo
        Inherits ComplexObject
        Private _UserName As String

        ''' <summary>
        ''' Ottiene e imposta il nome utente.
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
        ''' Ottiene o imposta una funzione che restituisce la password.
        ''' </summary>
        Friend Property PasswordAccessor As Func(Of String)

        ''' <summary>
        ''' Ottiene e imposta la password.
        ''' </summary>
        <Display(Name:="PasswordLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <Required()> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get
            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)

                ' Non archiviare la password in un campo privato poiché non deve essere memorizzata in testo normale.
                ' L'oggetto PasswordAccessor fornito funge invece da archivio di backup per il valore.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        Private _RememberMe As Boolean

        ''' <summary>
        ''' Ottiene e imposta il valore che indica se le informazioni di autenticazione dell'utente devono essere registrate per accessi futuri.
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
        ''' Ottiene o imposta l'operazione di accesso corrente.
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
        ''' Ottiene un valore che indica se è in corso l'accesso dell'utente.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property IsLoggingIn() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentLoginOperation)) AndAlso (Not Me.CurrentLoginOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Ottiene un valore che indica se l'utente può effettuare l'accesso.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property CanLogIn As Boolean
            Get
                Return (Not Me.IsLoggingIn)
            End Get
        End Property

        ''' <summary>
        ''' Genera notifiche di modifica delle proprietà correlate all'operazione in caso di modifica dell'operazione di accesso corrente.
        ''' </summary>
        Private Sub CurrentLoginOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsLoggingIn")
            Me.RaisePropertyChanged("CanLogIn")
        End Sub

        ''' <summary>
        ''' Crea una nuova istanza di <see cref="LoginParameters"/> utilizzando i dati archiviati in questa entità.
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, Me.RememberMe, Nothing)
        End Function
    End Class
End Namespace