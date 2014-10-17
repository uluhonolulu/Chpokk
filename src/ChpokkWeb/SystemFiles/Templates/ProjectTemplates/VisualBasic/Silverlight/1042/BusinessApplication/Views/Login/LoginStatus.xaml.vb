Imports System.ComponentModel
Imports System.Globalization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Namespace LoginUI
    ''' <summary>
    ''' 현재 로그인 상태를 표시하고 로그인 및 로그아웃을 허용하는 <see cref="UserControl"/> 클래스입니다.
    ''' </summary>
    Partial Public Class LoginStatus
        Inherits UserControl

        ''' <summary>
        ''' 새 <see cref="LoginStatus"/> 인스턴스를 만듭니다.
        ''' </summary>
        Public Sub New()
            Me.InitializeComponent()

            If DesignerProperties.IsInDesignTool Then
                VisualStateManager.GoToState(Me, "loggedOut", True)
            Else
                Me.DataContext = WebContext.Current
                AddHandler WebContext.Current.Authentication.LoggedIn, AddressOf Authentication_LoggedIn
                AddHandler WebContext.Current.Authentication.LoggedOut, AddressOf Authentication_LoggedOut
                Me.UpdateLoginState()
            End If
        End Sub

        Private Sub LoginButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            Dim loginWindow As New LoginRegistrationWindow()
            loginWindow.Show()
        End Sub

        Private Sub LogoutButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
            WebContext.Current.Authentication.Logout(AddressOf Me.HandleLogoutOperationErrors, Nothing)
        End Sub

        Private Sub HandleLogoutOperationErrors(ByVal logoutOperation As LogoutOperation)
            If logoutOperation.HasError Then
                ErrorWindow.CreateNew(logoutOperation.Error)
                logoutOperation.MarkErrorAsHandled()
            End If
        End Sub

        Private Sub Authentication_LoggedIn(ByVal sender As Object, ByVal e As AuthenticationEventArgs)
            Me.UpdateLoginState()
        End Sub

        Private Sub Authentication_LoggedOut(ByVal sender As Object, ByVal e As AuthenticationEventArgs)
            Me.UpdateLoginState()
        End Sub

        Private Sub UpdateLoginState()
            If WebContext.Current.User.IsAuthenticated Then
                Me.welcomeText.Text = String.Format(CultureInfo.CurrentUICulture, ApplicationStrings.WelcomeMessage, WebContext.Current.User.DisplayName)
            Else
                Me.welcomeText.Text = ApplicationStrings.AuthenticatingMessage
            End If

            If TypeOf (WebContext.Current.Authentication) Is WindowsAuthentication Then
                VisualStateManager.GoToState(Me, "windowsAuth", True)
            Else
                VisualStateManager.GoToState(Me, If(WebContext.Current.User.IsAuthenticated, "loggedIn", "loggedOut"), True)
            End If
        End Sub
    End Class
End Namespace