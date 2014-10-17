Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace LoginUI
    ''' <summary>
    ''' この内部エンティティは、UI コントロール (DataForm と検証エラーを表示するラベル) とユーザーが入力したログオン資格情報の間のバインディングを簡単にするために使用されます。
    ''' </summary>
    Public Class LoginInfo
        Inherits ComplexObject
        Private _UserName As String

        ''' <summary>
        ''' ユーザー名を取得および設定します。
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
        ''' パスワードを返す関数を取得または設定します。
        ''' </summary>
        Friend Property PasswordAccessor As Func(Of String)

        ''' <summary>
        ''' パスワードを取得および設定します。
        ''' </summary>
        <Display(Name:="PasswordLabel", ResourceType:=GetType(RegistrationDataResources))> _
        <Required()> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get
            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)

                ' パスワードをプレーン テキストでメモリに保存することはできないため、パスワードをプライベート フィールドに保存しないでください。
                ' 代わりに、指定された PasswordAccessor が値のバッキング ストアとして機能します。

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        Private _RememberMe As Boolean

        ''' <summary>
        ''' 今後のログインのためにユーザーの認証情報を記録するかどうかを示す値を取得および設定します。
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
        ''' 現在のログイン操作を取得または設定します。
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
        ''' ユーザーが現在ログインしているかどうかを示す値を取得します。
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property IsLoggingIn() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentLoginOperation)) AndAlso (Not Me.CurrentLoginOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' ユーザーが現在ログインできるかどうかを示す値を取得します。
        ''' </summary>
        <Display(AutoGenerateField:=False)>
        Public ReadOnly Property CanLogIn As Boolean
            Get
                Return (Not Me.IsLoggingIn)
            End Get
        End Property

        ''' <summary>
        ''' 現在のログイン操作が変更された場合、操作に関連するプロパティの変更通知を送信します。
        ''' </summary>
        Private Sub CurrentLoginOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsLoggingIn")
            Me.RaisePropertyChanged("CanLogIn")
        End Sub

        ''' <summary>
        ''' このエンティティに保存されたデータを使用して、新しい <see cref="LoginParameters"/> インスタンスを作成します。
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, Me.RememberMe, Nothing)
        End Function
    End Class
End Namespace