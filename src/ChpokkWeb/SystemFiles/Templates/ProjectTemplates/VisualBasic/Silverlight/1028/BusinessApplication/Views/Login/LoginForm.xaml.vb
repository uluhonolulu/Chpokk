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
    ''' 呈現登入欄位並處理登入程序的表單。
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' 建立新 <see cref="LoginForm"/> 執行個體。
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' 將此控制項的 DataContext 設定為 LoginInfo 執行個體以便於繫結。
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' 設定目前 <see cref="LoginForm"/> 的父視窗。
        ''' </summary>
        ''' <param name="window">要用來當做父代的視窗。</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' 處理 <see cref="DataForm.AutoGeneratingField"/> 來提供 PasswordAccessor。
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
        ''' 送出 <see cref="LoginOperation"/> 到伺服器
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' 我們必須強制驗證，因為我們不是使用 DataForm 的標準 [確定] 按鈕。
            ' 如果不確認表單是否有效，則萬一實體無效，便可能在叫用作業時發生例外狀況。
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' <see cref="LoginOperation"/> 的完成處理常式。
        ''' 如果作業成功，便會關閉視窗。
        ''' 如果有錯誤，便顯示 <see cref="ErrorWindow"/> 並將此錯誤標示為已處理。
        ''' 如果並未取消，但登入失敗，那一定是因為認證不正確，所以會加入驗證錯誤來通知使用者。
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
        ''' 切換到登入表單。
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' 如果登入作業正在進行而且可以取消，請取消。
        ''' 否則即關閉視窗。
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' 將 Esc 對應到取消按鈕、Enter 對應到確定按鈕。
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' 將焦點設定到使用者名稱文字方塊。
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace