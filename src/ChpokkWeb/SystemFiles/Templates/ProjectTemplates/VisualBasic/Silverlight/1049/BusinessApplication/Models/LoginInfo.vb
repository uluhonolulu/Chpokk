Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace LoginUI
    ''' <summary>
    ''' Эта внутренняя сущность служит для упрощения связывания элементов пользовательского интерфейса (DataForm и метка отображают ошибку проверки) и учетных данных входа, введенных пользователем.
    ''' </summary>
    Public Class LoginInfo
        Inherits ComplexObject
        Private _UserName As String

        ''' <summary>
        ''' Возвращает и задает имя пользователя.
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
        ''' Возвращает или задает функцию, возвращающую пароль.
        ''' </summary>
        Friend Property PasswordAccessor As Func(Of String)

        ''' <summary>
        ''' Возвращает и задает пароль.
        ''' </summary>
        <Display(Name:="PasswordLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <Required()> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get
            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)

                ' Не сохраняйте пароль в скрытых полях, так как он не должен храниться в памяти в виде обычного текста.
                ' Вместо этого для сохранения значения используйте объект PasswordAccessor.

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        Private _RememberMe As Boolean

        ''' <summary>
        ''' Возвращает и задает значение, указывающее, нужно ли сохранить данные проверки подлинности пользователя для использования в будущем.
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
        ''' Возвращает или задает текущую операцию входа.
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
        ''' Возвращает значение, указывающее, производит ли пользователь в настоящее время вход.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property IsLoggingIn() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentLoginOperation)) AndAlso (Not Me.CurrentLoginOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' Возвращает значение, указывающее, произвел ли пользователь в настоящее время вход.
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property CanLogIn As Boolean
            Get
                Return (Not Me.IsLoggingIn)
            End Get
        End Property

        ''' <summary>
        ''' Вызывает уведомления, связанные с изменением свойств операции входа в процессе ее изменения.
        ''' </summary>
        Private Sub CurrentLoginOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsLoggingIn")
            Me.RaisePropertyChanged("CanLogIn")
        End Sub

        ''' <summary>
        ''' Создает новый экземпляр класса <see cref="LoginParameters"/> на основе данных, хранящихся в этой сущности.
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, Me.RememberMe, Nothing)
        End Function
    End Class
End Namespace