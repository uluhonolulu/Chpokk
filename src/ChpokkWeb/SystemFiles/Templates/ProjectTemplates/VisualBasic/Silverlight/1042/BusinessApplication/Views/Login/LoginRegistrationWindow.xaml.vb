Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls

Namespace LoginUI
    ''' <summary>
    ''' 등록 프로세스를 제어하는 <see cref="ChildWindow"/> 클래스입니다.
    ''' </summary>
    Partial Public Class LoginRegistrationWindow
        Inherits ChildWindow

        Private possiblyPendingOperations As IList(Of OperationBase) = New List(Of OperationBase)()

        ''' <summary>
        ''' 새 <see cref="LoginRegistrationWindow"/> 인스턴스를 만듭니다.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            Me.registrationForm.SetParentWindow(Me)
            Me.loginForm.SetParentWindow(Me)

            AddHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            AddHandler Me.LayoutUpdated, AddressOf Me.UpdateTitle
        End Sub

        ''' <summary>
        ''' "AtLogin" 상태로 전환하여 이 구성 요소에 대한 <see cref="VisualStateManager"/>를 초기화합니다.
        ''' </summary>
        Private Sub GoToInitialState(ByVal sender As Object, ByVal eventArgs As EventArgs)
            RemoveHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            VisualStateManager.GoToState(Me, "AtLogin", False)
        End Sub

        ''' <summary>
        ''' 창이 열리면 표시 상태와 포커스가 올바른지 확인합니다.
        ''' </summary>
        Protected Overrides Sub OnOpened()
            MyBase.OnOpened()
            Me.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' 현재 표시된 패널(등록/로그인)에 따라 창 제목을 업데이트합니다.
        ''' </summary>
        Private Sub UpdateTitle(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me.Title = If(Me.registrationForm.Visibility = Visibility.Visible, _
                            ApplicationStrings.RegistrationWindowTitle, _
                            ApplicationStrings.LoginWindowTitle)
        End Sub

        ''' <summary>
        ''' <paramref name="operation"/>이 완료되었거나 취소할 수 있는 경우에만 닫을 수 있음을 <see cref="LoginRegistrationWindow"/> 창에 알립니다.
        ''' </summary>
        ''' <param name="operation">모니터링할 보류 중인 작업입니다.</param>
        Public Sub AddPendingOperation(ByVal operation As OperationBase)
            Me.possiblyPendingOperations.Add(operation)
        End Sub

        ''' <summary>
        ''' <see cref="VisualStateManager"/>가 "AtLogin" 상태로 변경됩니다.
        ''' </summary>
        Public Overridable Sub NavigateToLogin()
            VisualStateManager.GoToState(Me, "AtLogin", True)
            Me.loginForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' <see cref="VisualStateManager"/>가 "AtRegistration" 상태로 변경됩니다.
        ''' </summary>
        Public Overridable Sub NavigateToRegistration()
            VisualStateManager.GoToState(Me, "AtRegistration", True)
            Me.registrationForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' 작업이 진행 중인 동안에는 창이 닫히지 않습니다.
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