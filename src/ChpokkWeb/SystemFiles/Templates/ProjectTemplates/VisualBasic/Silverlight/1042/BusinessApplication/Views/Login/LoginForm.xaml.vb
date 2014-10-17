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
    ''' 로그인 필드를 표시하고 로그인 프로세스를 처리하는 폼입니다.
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' 새 <see cref="LoginForm"/> 인스턴스를 만듭니다.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' 쉽게 바인딩할 수 있도록 이 컨트롤의 DataContext를 LoginInfo 인스턴스로 설정합니다.
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' 현재 <see cref="LoginForm"/>에 대한 부모 창을 설정합니다.
        ''' </summary>
        ''' <param name="window">부모로 사용할 창입니다.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' PasswordAccessor를 제공하도록 <see cref="DataForm.AutoGeneratingField"/>를 처리합니다.
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
        ''' <see cref="LoginOperation"/>을 서버로 전송합니다.
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' DataForm에서 표준 확인 단추를 사용하지 않기 때문에 유효성 검사를 강제로 수행해야 합니다.
            ' 폼이 올바른지 확인하지 않으면 엔터티가 잘못된 경우 작업을 호출하는 동안 예외가 발생합니다.
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' <see cref="LoginOperation"/>에 대한 완료 처리기입니다.
        ''' 작업이 성공하면 창을 닫습니다.
        ''' 오류가 있는 경우 <see cref="ErrorWindow"/>가 표시되고 오류가 처리된 것으로 표시됩니다.
        ''' 취소하지 않았지만 로그인이 실패한 경우 자격 증명이 올바르지 않아서 사용자에게 알리기 위한 유효성 검사 오류가 추가되기 때문입니다.
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
        ''' 등록 폼으로 전환합니다.
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' 로그인 작업이 진행 중이고 취소할 수 있는 경우 로그인 작업을 취소합니다.
        ''' 그렇지 않으면 창을 닫습니다.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Esc 키를 취소 단추에 매핑하고 Enter 키를 확인 단추에 매핑합니다.
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' 사용자 이름 입력란에 포커스를 설정합니다.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace