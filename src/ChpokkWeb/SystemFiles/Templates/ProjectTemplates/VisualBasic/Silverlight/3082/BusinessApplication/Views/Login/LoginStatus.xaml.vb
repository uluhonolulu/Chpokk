﻿Imports System.ComponentModel
Imports System.Globalization
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Data

Namespace LoginUI
    ''' <summary>
    ''' Clase <see cref="UserControl"/> que muestra el estado actual de inicio de sesión y permite iniciar y cerrar sesión.
    ''' </summary>
    Partial Public Class LoginStatus
        Inherits UserControl

        ''' <summary>
        ''' Crea una nueva instancia de <see cref="LoginStatus"/>.
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