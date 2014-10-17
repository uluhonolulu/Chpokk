Imports System
Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices

Namespace Web

    Partial Public Class RegistrationData
        Private _CurrentOperation As OperationBase

        ''' <summary>
        ''' パスワードを返す関数を取得または設定します。
        ''' </summary>
        Friend Property PasswordAccessor() As Func(Of String)

        ''' <summary>
        ''' パスワードを取得および設定します。
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=3, Name:="PasswordLabel", Description:="PasswordDescription", ResourceType:=GetType(RegistrationDataResources))> _
        <RegularExpression("^.*[^a-zA-Z0-9].*$", ErrorMessageResourceName:="ValidationErrorBadPasswordStrength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <StringLength(50, MinimumLength:=7, ErrorMessageResourceName:="ValidationErrorBadPasswordLength", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        Public Property Password() As String
            Get
                Return If((Me.PasswordAccessor Is Nothing), String.Empty, Me.PasswordAccessor.Invoke())
            End Get

            Set(ByVal value As String)
                Me.ValidateProperty("Password", value)
                Me.CheckPasswordConfirmation()

                ' パスワードをプレーン テキストでメモリに保存することはできないため、パスワードをプライベート フィールドに保存しないでください。
                ' 代わりに、指定された PasswordAccessor が値のバッキング ストアとして機能します。

                Me.RaisePropertyChanged("Password")
            End Set
        End Property

        ''' <summary>
        ''' パスワードの確認を返す関数を取得または設定します。
        ''' </summary>
        Friend Property PasswordConfirmationAccessor() As Func(Of String)

        ''' <summary>
        ''' パスワードの確認文字列を取得および設定します。
        ''' </summary>
        <Required(ErrorMessageResourceName:="ValidationErrorRequiredField", ErrorMessageResourceType:=GetType(ValidationErrorResources))> _
        <Display(Order:=4, Name:="PasswordConfirmationLabel", ResourceType:=GetType(RegistrationDataResources))> _
        Public Property PasswordConfirmation() As String
            Get
                Return If((Me.PasswordConfirmationAccessor Is Nothing), String.Empty, Me.PasswordConfirmationAccessor.Invoke())
            End Get

            Set(ByVal value As String)
                Me.ValidateProperty("PasswordConfirmation", value)
                Me.CheckPasswordConfirmation()

                ' パスワードをプレーン テキストでメモリに保存することはできないため、パスワードをプライベート フィールドに保存しないでください。
                ' 代わりに、指定された PasswordAccessor が値のバッキング ストアとして機能します。

                Me.RaisePropertyChanged("PasswordConfirmation")
            End Set
        End Property

        ''' <summary>
        ''' 現在の登録操作またはログイン操作を取得または設定します。
        ''' </summary>
        Friend Property CurrentOperation() As OperationBase
            Get
                Return _CurrentOperation
            End Get
            Set(ByVal value As OperationBase)
                If Not Object.Equals(_CurrentOperation, value) Then
                    If Not IsNothing(_CurrentOperation) Then
                        RemoveHandler _CurrentOperation.Completed, AddressOf Me.CurrentOperationChanged
                    End If

                    _CurrentOperation = value

                    If Not IsNothing(_CurrentOperation) Then
                        AddHandler _CurrentOperation.Completed, AddressOf Me.CurrentOperationChanged
                    End If

                    Me.CurrentOperationChanged(Me, EventArgs.Empty)
                End If
            End Set
        End Property

        ''' <summary>
        ''' ユーザーが現在登録されているか、またはログインしているかを示す値を取得します。
        ''' </summary>
        <Display(AutoGenerateField:=False)> _
        Public ReadOnly Property IsRegistering() As Boolean
            Get
                Return (Not IsNothing(Me.CurrentOperation)) AndAlso (Not Me.CurrentOperation.IsComplete)
            End Get
        End Property

        ''' <summary>
        ''' 現在の操作が変更された場合のヘルパー メソッドです。
        ''' 適切なプロパティの変更通知を送信するために使用されます。
        ''' </summary>
        ''' <param name="sender">イベントの送信者です。</param>
        ''' <param name="e">event の引数です。</param>
        Private Sub CurrentOperationChanged(ByVal sender As Object, ByVal e As EventArgs)
            Me.RaisePropertyChanged("IsRegistering")
        End Sub

        ''' <summary>
        ''' パスワードとパスワードの確認が一致することを確認します。
        ''' 一致しない場合は、検証エラーが追加されます。
        ''' </summary>
        Private Sub CheckPasswordConfirmation()
            ' いずれかのパスワードが入力されていない場合、フィールドが同じかどうかのテストは行われません。
            ' Required 属性により、両方のフィールドに必ず値が入力されるようになります。
            If String.IsNullOrWhiteSpace(Me.Password) OrElse String.IsNullOrWhiteSpace(Me.PasswordConfirmation) Then
                Exit Sub
            End If

            ' 値が異なる場合は、両方のメンバーが指定された検証エラーを追加します。
            If Me.Password <> Me.PasswordConfirmation Then
                Me.ValidationErrors.Add(New ValidationResult(ValidationErrorResources.ValidationErrorPasswordConfirmationMismatch, New String() {"PasswordConfirmation", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' UserName 値の入力後、ロジックを実行します
        ''' </summary>
        ''' <param name="userName">入力されたユーザー名です。</param>
        ''' <remarks>
        ''' 値が完全に入力されたことをフォームが示すようにします。
        ''' OnUserNameChanged メソッドを使用すると、ユーザーがフォームで値の入力を完了する前に途中で呼び出すことができます。
        ''' </remarks>
        Friend Sub UserNameEntered(ByVal userName As String)
            ' フレンドリ名が指定されていない場合は、新しいエンティティの UserName と一致するように FriendlyName が自動入力されます
            If String.IsNullOrWhiteSpace(Me.FriendlyName) Then
                Me.FriendlyName = userName
            End If
        End Sub

        ''' <summary>
        ''' このエンティティのデータで初期化された新しい <see cref="LoginParameters"/> を作成します (IsPersistent の既定値は false になります)。
        ''' </summary>
        Public Function ToLoginParameters() As LoginParameters
            Return New LoginParameters(Me.UserName, Me.Password, False, Nothing)
        End Function
    End Class
End Namespace