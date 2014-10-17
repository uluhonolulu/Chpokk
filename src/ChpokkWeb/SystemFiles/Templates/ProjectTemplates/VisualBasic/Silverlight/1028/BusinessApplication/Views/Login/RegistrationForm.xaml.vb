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
    ''' 呈現 <see cref="RegistrationData"/> 並執行註冊程序的表單。
    ''' </summary>
    Partial Public Class RegistrationForm
        Inherits StackPanel

        Private parentWindow As LoginRegistrationWindow
        Private registrationData As New RegistrationData()
        Private userRegistrationContext As New UserRegistrationContext()
        Private userNameTextBox As TextBox

        ''' <summary>
        ''' 建立新 <see cref="RegistrationForm"/> 執行個體。
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            ' 將此控制項的 DataContext 設定為 Registration 執行個體以便於繫結。
            Me.DataContext = registrationData
        End Sub

        ''' <summary>
        ''' 設定目前 <see cref="RegistrationForm"/> 的父視窗。
        ''' </summary>
        ''' <param name="window">要用來當做父代的視窗。</param>
        Public Sub SetParentWindow(ByVal window As LoginRegistrationWindow)
            Me.parentWindow = window
        End Sub

        ''' <summary>
        ''' 於欄位產生時串聯起 Password 和 PasswordConfirmation 存取子。
        ''' 同時將 [Question] 欄位繫結到充滿安全性問題的 ComboBox，並且處理 UserName TextBox 的 LostFocus 事件。
        ''' </summary>
        Private Sub RegisterForm_AutoGeneratingField(ByVal dataForm As Object, ByVal e As DataFormAutoGeneratingFieldEventArgs)
            ' 將所有欄位置於加入模式
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
        ''' UserName TextBox 失去焦點時的回呼。
        ''' 呼叫註冊資料以便對邏輯進行處理，可能需設定 [FriendlyName] 欄位。
        ''' </summary>
        ''' <param name="sender">事件發送者。</param>
        ''' <param name="e">event 引數。</param>
        Private Sub UserNameLostFocus(ByVal sender As Object, ByVal e As RoutedEventArgs)
            registrationData.UserNameEntered(DirectCast(sender, TextBox).Text)
        End Sub

        ''' <summary>
        ''' 將指定的繫結設定成使用 TargetNullValueConverter。
        ''' </summary>
        ''' <param name="comboBoxBinding">要設定的 binding</param>
        Private Sub ConfigureComboBoxBinding(ByVal comboBoxBinding As Binding)
            comboBoxBinding.Converter = New TargetNullValueConverter()
        End Sub

        ''' <summary>
        ''' 傳回 <see cref="SecurityQuestions" /> 中所定義資源字串的清單。
        ''' </summary>
        Private Shared Function GetSecurityQuestions() As IEnumerable(Of String)
            ' 使用反射來取得所有當地語系化的安全性問題
            Return From propertyInfo In GetType(SecurityQuestions).GetProperties() _
                   Where propertyInfo.PropertyType.Equals(GetType(String)) _
                   Select DirectCast(propertyInfo.GetValue(Nothing, Nothing), String)
        End Function

        ''' <summary>
        ''' 送出新註冊。
        ''' </summary>
        Private Sub RegisterButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            ' 我們必須強制驗證，因為我們不是使用 DataForm 的標準 [確定] 按鈕。
            ' 如果不確認表單是否有效，則萬一實體無效，便可能在叫用作業時發生例外狀況。
            If Me.registerForm.ValidateItem() Then
                registrationData.CurrentOperation = userRegistrationContext.CreateUser(registrationData, registrationData.Password, AddressOf Me.RegistrationOperation_Completed, Nothing)
                parentWindow.AddPendingOperation(registrationData.CurrentOperation)
            End If
        End Sub

        ''' <summary>
        ''' 註冊作業的完成處理常式。
        ''' 如果有錯誤，便向使用者顯示 <see cref="ErrorWindow"/>。
        ''' 否則，它將觸發會自動登入剛註冊之使用者的登入作業。
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
        ''' 發生於註冊及登入嘗試成功後的登入作業的完成處理常式。  
        ''' 這樣將會關閉視窗。如果作業失敗，<see cref="ErrorWindow"/> 將會顯示錯誤訊息。
        ''' </summary>
        ''' <param name="loginOperation">已完成的 <see cref="LoginOperation"/>。</param>
        Private Sub LoginOperation_Completed(ByVal loginOperation As LoginOperation)
            If Not loginOperation.IsCanceled Then
                parentWindow.DialogResult = True

                If loginOperation.HasError Then
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, loginOperation.Error.Message))
                    loginOperation.MarkErrorAsHandled()
                ElseIf Not loginOperation.LoginSuccess Then
                    ' 作業成功，但實際登入失敗
                    ErrorWindow.CreateNew(String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorLoginAfterRegistrationFailed, ErrorResources.ErrorBadUserNameOrPassword))
                End If
            End If
        End Sub

        ''' <summary>
        ''' 切換到登入視窗。
        ''' </summary>
        Private Sub BackToLogin_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            parentWindow.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' 如果註冊或登入作業正在進行而且可以取消，便將它取消。
        ''' 否則即關閉視窗。
        ''' </summary>
        Private Sub CancelButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            If (Not IsNothing(registrationData.CurrentOperation)) AndAlso registrationData.CurrentOperation.CanCancel Then
                registrationData.CurrentOperation.Cancel()
            Else
                parentWindow.DialogResult = False
            End If
        End Sub

        ''' <summary>
        ''' 將 Esc 對應到取消按鈕、Enter 對應到確定按鈕。
        ''' </summary>
        Private Sub RegistrationForm_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
            If e.Key = Key.Escape Then
                Me.CancelButton_Click(sender, e)
            ElseIf e.Key = Key.Enter And Me.registerButton.IsEnabled Then
                Me.RegisterButton_Click(sender, e)
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