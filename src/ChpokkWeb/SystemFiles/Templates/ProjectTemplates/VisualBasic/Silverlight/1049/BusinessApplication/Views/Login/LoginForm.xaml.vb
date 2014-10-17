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
    ''' Форма, которая содержит поля для входа и обрабатывает вход.
    ''' </summary>
    Partial Public Class LoginForm
        Inherits StackPanel
        Private parentWindow As LoginRegistrationWindow
        Private loginInfo As New LoginInfo()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Создает новый экземпляр класса <see cref="LoginForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Задайте свойству DataContext этого элемента управления экземпляр LoginInfo, чтобы упростить привязку.
            Me.DataContext = loginInfo
        End Sub

        ''' <summary>
        ''' Устанавливает родительское окно для текущего окна <see cref="LoginForm"/>.
        ''' </summary>
        ''' <param name="window">Окно, которое будет использоваться в качестве родительского.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Обрабатывает поле <see cref="DataForm.AutoGeneratingField"/>, чтобы получить значение PasswordAccessor.
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
        ''' Отправляет <see cref="LoginOperation"/> на сервер
        ''' </summary>
        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            ' Необходима принудительная проверка, поскольку стандартная кнопка "ОК" для DataForm не используется.
            ' Без проверки правильности формы в случае, если сущность содержит неверные данные, можно получить исключение (exception) при вызове операции.
            If Me.loginForm.ValidateItem() Then
                loginInfo.CurrentLoginOperation = WebContext.Current.Authentication.Login(loginInfo.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(loginInfo.CurrentLoginOperation)
            End If
        End Sub

        ''' <summary>
        ''' Обработчик завершения для операции <see cref="LoginOperation"/>.
        ''' Если операция завершилась успешно, окно закрывается.
        ''' Если обнаружена ошибка, отображается окно <see cref="ErrorWindow"/>, а ошибка помечается как обработанная.
        ''' Если операция не была отменена, но вход завершился с ошибкой, вероятно, что учетные данные были введены неверно, поэтому уведомление об ошибке проверки подлинности направляется пользователю.
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
        ''' Переключение на форму регистрации.
        ''' </summary>
        Private Sub RegisterNow_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToRegistration()
        End Sub

        ''' <summary>
        ''' Если выполняется операция входа, которую можно отменить, она отменяется.
        ''' В противном случае окно закрывается.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As EventArgs)
            If (Not IsNothing(loginInfo.CurrentLoginOperation)) AndAlso loginInfo.CurrentLoginOperation.CanCancel Then
                loginInfo.CurrentLoginOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Сопоставляет клавишу Esc с кнопкой "Отмена", а клавишу ВВОД с кнопкой "ОК".
        ''' </summary>
        Private Sub LoginForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter AndAlso Me.loginButton.IsEnabled Then
                Me.LoginButton_Click(sender, e)
            End If
        End Sub

        ''' <summary>
        ''' Устанавливает фокус в текстовом поле имени пользователя.
        ''' </summary>
        Public Sub SetInitialFocus()
            Me.userNameTextBox.Focus()
        End Sub
    End Class
End Namespace