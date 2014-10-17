Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Input
Imports $safeprojectname$.Controls

Namespace LoginUI
    ''' <summary>
    ''' ログイン フィールドを表し、ログイン プロセスを処理するフォームです。
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' 新しい <see cref="LoginForm"/> インスタンスを作成します。
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' 簡単にバインディングできるように、このコントロールの DataContext を LoginInfo インスタンスに設定します。
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' 現在の <see cref="LoginForm"/> の親ウィンドウを設定します。
        ''' </summary>
        ''' <param name="window">親として使用するウィンドウです。</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' <see cref="DataForm.AutoGeneratingField"/> を処理して PasswordAccessor を指定します。
        ''' </summary>
        Private Sub LoginForm_AutoGeneratingField(ByVal sender As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            If e.PropertyName = "UserName" Then
                Me.userNameTextBox = DirectCast(e.Field.Content, TextBox)
            ElseIf e.PropertyName = "Password" Then
                Dim passwordBox As New PasswordBox()
                e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty)
                loginInfo.PasswordAccessor = Function() passwordBox.Password
            End If
        End Sub

        ''' <summary>
        ''' <see cref="LoginOperation"/> をサーバーに送信します
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' DataForm から標準の [OK] ボタンを使用していないため、強制的に検証する必要があります。
            ' フォームが有効であることを確認しないと、エンティティが無効な場合、操作を呼び出す例外が発生します。
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' <see cref="LoginOperation"/> の完了ハンドラーです。
        ''' 操作が成功した場合、ウィンドウを閉じます。
        ''' エラーがある場合、<see cref="ErrorWindow"/> を表示し、エラーを処理済みとしてマークします。
        ''' キャンセルされずにログインに失敗した場合、資格情報が間違っており、ユーザーに通知するために検証エラーが追加されたことが原因です。
        ''' </summary>        
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If loginOperation.LoginSuccess Then
                parentWindow.DialogResult = True
            ElseIf loginOperation.HasError Then
                ErrorWindow.CreateNew(loginOperation.Error)
                loginOperation.MarkErrorAsHandled()
            ElseIf Not loginOperation.IsCanceled Then
                loginInfo.ValidationErrors.Add(New ValidationResult(ErrorResources.ErrorBadUserNameOrPassword, New String() {"UserName", "Password"}))
            End If
        End Sub

        ''' <summary>
        ''' 登録フォームに切り替えます。
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' ログイン操作の実行中で、キャンセルできる場合は、キャンセルします。
        ''' それ以外の場合は、ウィンドウを閉じます。
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Esc キーを [キャンセル] ボタン、Enter キーを [OK] ボタンにマップします。
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' フォーカスをユーザー名テキスト ボックスに設定します。
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace