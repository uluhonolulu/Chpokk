Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls

Namespace LoginUI
    ''' <summary>
    ''' 控制註冊程序的 <see cref="ChildWindow"/> 類別。
    ''' </summary>
    Partial Public Class LoginRegistrationWindow
        Inherits ChildWindow

        Private possiblyPendingOperations As IList(Of OperationBase) = New List(Of OperationBase)()

        ''' <summary>
        ''' 建立新 <see cref="LoginRegistrationWindow"/> 執行個體。
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            Me.registrationForm.SetParentWindow(Me)
            Me.loginForm.SetParentWindow(Me)

            AddHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            AddHandler Me.LayoutUpdated, AddressOf Me.UpdateTitle
        End Sub

        ''' <summary>
        ''' 初始化此元件的 <see cref="VisualStateManager"/>，方法是將它置於 "AtLogin" 狀態
        ''' </summary>
        Private Sub GoToInitialState(ByVal sender As Object, ByVal eventArgs As EventArgs)
            RemoveHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            VisualStateManager.GoToState(Me, "AtLogin", False)
        End Sub

        ''' <summary>
        ''' 確保視窗開啟時可見狀態和焦點正確無誤。
        ''' </summary>
        Protected Overrides Sub OnOpened()
            MyBase.OnOpened()
            Me.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' 依據目前顯示的是哪個面板 (註冊 / 登入) 來更新視窗標題。
        ''' </summary>
        Private Sub UpdateTitle(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me.Title = If(Me.registrationForm.Visibility = Visibility.Visible, _
                            ApplicationStrings.RegistrationWindowTitle, _
                            ApplicationStrings.LoginWindowTitle)
        End Sub

        ''' <summary>
        ''' 通知 <see cref="LoginRegistrationWindow"/> 視窗它只能在 <paramref name="operation"/> 已完成或可以取消的情況下關閉。
        ''' </summary>
        ''' <param name="operation">要監視的暫止作業</param>
        Public Sub AddPendingOperation(ByVal operation As OperationBase)
            Me.possiblyPendingOperations.Add(operation)
        End Sub

        ''' <summary>
        ''' 使 <see cref="VisualStateManager"/> 變更為 "AtLogin" 狀態。
        ''' </summary>
        Public Overridable Sub NavigateToLogin()
            VisualStateManager.GoToState(Me, "AtLogin", True)
            Me.loginForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' 使 <see cref="VisualStateManager"/> 變更為 "AtRegistration" 狀態。
        ''' </summary>
        Public Overridable Sub NavigateToRegistration()
            VisualStateManager.GoToState(Me, "AtRegistration", True)
            Me.registrationForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' 防止視窗在作業進行時關閉
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