Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls

Namespace LoginUI
    ''' <summary>
    ''' Classe <see cref="ChildWindow"/> qui contrôle le processus d'inscription.
    ''' </summary>
    Partial Public Class LoginRegistrationWindow
        Inherits ChildWindow

        Private possiblyPendingOperations As IList(Of OperationBase) = New List(Of OperationBase)()

        ''' <summary>
        ''' Crée une nouvelle instance <see cref="LoginRegistrationWindow"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            Me.registrationForm.SetParentWindow(Me)
            Me.loginForm.SetParentWindow(Me)

            AddHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            AddHandler Me.LayoutUpdated, AddressOf Me.UpdateTitle
        End Sub

        ''' <summary>
        ''' Initialise le <see cref="VisualStateManager"/> pour ce composant en le plaçant dans l'état « AtLogin »
        ''' </summary>
        Private Sub GoToInitialState(ByVal sender As Object, ByVal eventArgs As EventArgs)
            RemoveHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            VisualStateManager.GoToState(Me, "AtLogin", False)
        End Sub

        ''' <summary>
        ''' Garantit que l'état visuel et le focus sont corrects lors de l'ouverture de la fenêtre.
        ''' </summary>
        Protected Overrides Sub OnOpened()
            MyBase.OnOpened()
            Me.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Met à jour le titre de la fenêtre en fonction du panneau (inscription/connexion) actuellement affiché.
        ''' </summary>
        Private Sub UpdateTitle(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me.Title = If(Me.registrationForm.Visibility = Visibility.Visible, _
                            ApplicationStrings.RegistrationWindowTitle, _
                            ApplicationStrings.LoginWindowTitle)
        End Sub

        ''' <summary>
        ''' Avertit la fenêtre <see cref="LoginRegistrationWindow"/> qu'elle peut se fermer uniquement si <paramref name="operation"/> est terminée ou peut être annulée.
        ''' </summary>
        ''' <param name="operation">Opération en attente à surveiller</param>
        Public Sub AddPendingOperation(ByVal operation As OperationBase)
            Me.possiblyPendingOperations.Add(operation)
        End Sub

        ''' <summary>
        ''' Entraîne le passage de <see cref="VisualStateManager"/> dans l'état « AtLogin ».
        ''' </summary>
        Public Overridable Sub NavigateToLogin()
            VisualStateManager.GoToState(Me, "AtLogin", True)
            Me.loginForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' Entraîne le passage de <see cref="VisualStateManager"/> dans l'état « AtRegistration ».
        ''' </summary>
        Public Overridable Sub NavigateToRegistration()
            VisualStateManager.GoToState(Me, "AtRegistration", True)
            Me.registrationForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' Empêche la fermeture de la fenêtre pendant que des opérations sont en cours
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