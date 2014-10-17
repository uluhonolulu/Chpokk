Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls

Namespace LoginUI
    ''' <summary>
    ''' Класс <see cref="ChildWindow"/>, управляющий процессом регистрации.
    ''' </summary>
    Partial Public Class LoginRegistrationWindow
        Inherits ChildWindow

        Private possiblyPendingOperations As IList(Of OperationBase) = New List(Of OperationBase)()

        ''' <summary>
        ''' Создает новый экземпляр класса <see cref="LoginRegistrationWindow"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            Me.registrationForm.SetParentWindow(Me)
            Me.loginForm.SetParentWindow(Me)

            AddHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            AddHandler Me.LayoutUpdated, AddressOf Me.UpdateTitle
        End Sub

        ''' <summary>
        ''' Инициализирует экземпляр <see cref="VisualStateManager"/> для этого компонента, переводя его в состояние "AtLogin"
        ''' </summary>
        Private Sub GoToInitialState(ByVal sender As Object, ByVal eventArgs As EventArgs)
            RemoveHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            VisualStateManager.GoToState(Me, "AtLogin", False)
        End Sub

        ''' <summary>
        ''' Обеспечивает правильность визуального состояния и фокуса при открытии окна.
        ''' </summary>
        Protected Overrides Sub OnOpened()
            MyBase.OnOpened()
            Me.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Обновляет заголовок окна в соответствии с текущей отображаемой панелью (регистрации или входа).
        ''' </summary>
        Private Sub UpdateTitle(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me.Title = If(Me.registrationForm.Visibility = Visibility.Visible, _
                            ApplicationStrings.RegistrationWindowTitle, _
                            ApplicationStrings.LoginWindowTitle)
        End Sub

        ''' <summary>
        ''' Уведомляет окно <see cref="LoginRegistrationWindow"/> о том, что его закрытие возможно только после завершения или отмены операции <paramref name="operation"/>.
        ''' </summary>
        ''' <param name="operation">Операция в режиме ожидания для наблюдения</param>
        Public Sub AddPendingOperation(ByVal operation As OperationBase)
            Me.possiblyPendingOperations.Add(operation)
        End Sub

        ''' <summary>
        ''' Вызывает переход <see cref="VisualStateManager"/> в состояние "AtLogin".
        ''' </summary>
        Public Overridable Sub NavigateToLogin()
            VisualStateManager.GoToState(Me, "AtLogin", True)
            Me.loginForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' Вызывает переход <see cref="VisualStateManager"/> в состояние "AtRegistration".
        ''' </summary>
        Public Overridable Sub NavigateToRegistration()
            VisualStateManager.GoToState(Me, "AtRegistration", True)
            Me.registrationForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' Предотвращает закрытие окна до завершения выполнения операции
        ''' </summary>
        Private Sub LoginWindow_Closing(ByVal sender As Object, ByVal eventArgs As CancelEventArgs) Handles Me.Closing
            For Each operation As OperationBase In Me.possiblyPendingOperations
                If Not operation.IsComplete Then
                    If operation.CanCancel Then
                        operation.Cancel()
                    Else
                        eventArgs.Cancel = True
                    End If
                End If
            Next
        End Sub
    End Class
End Namespace