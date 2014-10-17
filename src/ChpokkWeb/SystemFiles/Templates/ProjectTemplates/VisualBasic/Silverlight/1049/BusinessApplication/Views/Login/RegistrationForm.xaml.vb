Imports System
Imports System.Collections
Imports System.Collections.Generic
Imports System.ComponentModel.DataAnnotations
Imports System.Linq
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data
Imports System.Windows.Input
Imports $safeprojectname$.Web

Namespace LoginUI
    ''' <summary>
    ''' Форма, которая отображает <see cref="RegistrationData"/> и выполняет процедуру регистрации.
    ''' </summary>
    Partial Public Class RegistrationForm
        Inherits StackPanel

        Private parentWindow As LoginRegistrationWindow
        Private registrationData As New RegistrationData()
        Private userRegistrationContext As New UserRegistrationContext()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' Создает новый экземпляр класса <see cref="RegistrationForm"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' Задает свойству DataContext этого элемента управления экземпляр Registration, чтобы упростить привязку.
            Me.DataContext = registrationData
        End Sub

        ''' <summary>
        ''' Устанавливает родительское окно для текущего окна <see cref="RegistrationForm"/>.
        ''' </summary>
        ''' <param name="window">Окно, которое будет использоваться в качестве родительского.</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' Связывает методы доступа к полям Password и PasswordConfirmation при создании этих полей.
        ''' Кроме того, связывает поле Question с элементом ComboBox, содержащим список секретных вопросов, и обрабатывает событие LostFocus для элемента управления TextBox с именем UserName.
        ''' </summary>
        Private Sub RegisterForm_AutoGeneratingField(ByVal dataForm As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            ' Переводит все поля в режим добавления
            e.Field.Mode = DataFieldMode.AddNew

            If e.PropertyName = "UserName" Then
                Me.userNameTextBox = DirectCast(e.Field.Content, TextBox)
                AddHandler Me.userNameTextBox.LostFocus, AddressOf UserNameLostFocus
            ElseIf e.PropertyName = "Password" Then
                Dim passwordBox As New PasswordBox()
                e.Field.ReplaceTextBox(passwordBox, PasswordBox.PasswordProperty)
                registrationData.PasswordAccessor = Function() passwordBox.Password
            ElseIf e.PropertyName = "PasswordConfirmation" Then
                Dim passwordConfirmationBox As New PasswordBox()
                e.Field.ReplaceTextBox(passwordConfirmationBox, PasswordBox.PasswordProperty)
                registrationData.PasswordConfirmationAccessor = Function() passwordConfirmationBox.Password
            ElseIf e.PropertyName = "Question" Then
                Dim questionComboBox As New ComboBox()
                questionComboBox.ItemsSource = RegistrationForm.GetSecurityQuestions()
                e.Field.ReplaceTextBox(questionComboBox, ComboBox.SelectedItemProperty, AddressOf Me.ConfigureComboBoxBinding)
            End If
        End Sub

        ''' <summary>
        ''' Обратный вызов. Вызывается, когда элемент управления TextBox с именем UserName теряет фокус.
        ''' Вызывается при регистрации данных, давая возможность обработать логику и, возможно, установить поле FriendlyName.
        ''' </summary>
        ''' <param name="sender">Отправитель события.</param>
        ''' <param name="e">Аргументы события.</param>
        Private Sub UserNameLostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
            registrationData.UserNameEntered(DirectCast(sender, TextBox).Text)
        End Sub

        ''' <summary>
        ''' Настроить указанную привязку на использование класса TargetNullValueConverter.
        ''' </summary>
        ''' <param name="comboBoxBinding">Настраиваемая привязка (binding)</param>
        Private Sub ConfigureComboBoxBinding(ByVal comboBoxBinding As Binding)
            comboBoxBinding.Converter = New TargetNullValueConverter()
        End Sub

        ''' <summary>
        ''' Возвращает список строк ресурсов, определенных в коллекции <see cref="SecurityQuestions" />.
        ''' </summary>
        Private Shared Function GetSecurityQuestions() As IEnumerable(Of String)
            ' С помощью отражения (reflection) получить все локализованные секретные вопросы
            Return From propertyInfo In GetType(SecurityQuestions).GetProperties() _
                   Where propertyInfo.PropertyType.Equals(GetType(String)) _
                   Select DirectCast(propertyInfo.GetValue(Nothing, Nothing), String)
        End Function

        ''' <summary>
        ''' Отправить данные для новой регистрации.
        ''' </summary>
        Private Sub RegisterButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' Необходима принудительная проверка, поскольку стандартная кнопка "ОК" для DataForm не используется.
            ' Без проверки правильности формы в случае, если сущность содержит неверные данные, можно получить исключение (exception) при вызове операции.
            If Me.registerForm.ValidateItem() Then
                registrationData.CurrentOperation = userRegistrationContext.CreateUser(registrationData, registrationData.Password, AddressOf Me.RegistrationOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(registrationData.CurrentOperation)
            End If
        End Sub

        ''' <summary>
        ''' Обработчик завершения для операции регистрации. 
        ''' При обнаружении ошибки пользователю выводится окно <see cref="ErrorWindow"/>.
        ''' В противном случае инициируется операция входа, которая автоматически производит вход для только что зарегистрированного пользователя.
        ''' </summary>
        Private Sub RegistrationOperation_Completed(ByVal operation As InvokeOperation(Of CreateUserStatus))
            If Not Operation.IsCanceled Then
                If operation.HasError Then
                    ErrorWindow.CreateNew(operation.Error)
                    operation.MarkErrorAsHandled()
                ElseIf operation.Value = CreateUserStatus.Success Then
                    registrationData.CurrentOperation = WebContext.Current.Authentication.Login(registrationData.ToLoginParameters(), AddressOf Me.LoginOperation_Completed, Nothing)
                    parentWindow.AddPendingOperation(registrationData.CurrentOperation)
                ElseIf operation.Value = CreateUserStatus.DuplicateUserName Then
                    registrationData.ValidationErrors.Add(New ValidationResult(ErrorResources.CreateUserStatusDuplicateUserName, New String() {"UserName"}))
                ElseIf operation.Value = CreateUserStatus.DuplicateEmail Then
                    registrationData.ValidationErrors.Add(New ValidationResult(ErrorResources.CreateUserStatusDuplicateEmail, New String() {"Email"}))
                Else
                    ErrorWindow.CreateNew(ErrorResources.ErrorWindowGenericError)
                End If
            End If
        End Sub

        ''' <summary>
        ''' Обработчик завершения для операции входа возникает после успешной регистрации и попытки входа.  
        ''' Закрывает окно. Если операция завершилась ошибкой, сообщение будет отображено в окне <see cref="ErrorWindow"/>.
        ''' </summary>
        ''' <param name="loginOperation">Завершаемая операция <see cref="LoginOperation"/>.</param>
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If Not loginOperation.IsCanceled Then
                parentWindow.DialogResult = True

                If loginOperation.HasError Then
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, loginOperation.Error.Message))
                    loginOperation.MarkErrorAsHandled()
                ElseIf Not loginOperation.LoginSuccess Then
                    ' Операция выполнена успешно, но вход не выполнен
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword))
                End If
            End If
        End Sub

        ''' <summary>
        ''' Переключается в окно входа.
        ''' </summary>
        Private Sub BackToLogin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Если выполняется операция регистрации или входа, которую можно отменить, она отменяется.
        ''' В противном случае окно закрывается.
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not IsNothing(registrationData.CurrentOperation)) AndAlso registrationData.CurrentOperation.CanCancel Then
                registrationData.CurrentOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' Сопоставляет клавишу Esc с кнопкой "Отмена", а клавишу ВВОД с кнопкой "ОК".
        ''' </summary>
        Private Sub RegistrationForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter And Me.registerButton.IsEnabled Then
                Me.RegisterButton_Click(sender, e)
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