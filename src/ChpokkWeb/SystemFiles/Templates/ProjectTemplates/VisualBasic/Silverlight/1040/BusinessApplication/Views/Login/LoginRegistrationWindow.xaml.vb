Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.ServiceModel.DomainServices.Client
Imports System.ServiceModel.DomainServices.Client.ApplicationServices
Imports System.Windows
Imports System.Windows.Controls

Namespace LoginUI
    ''' <summary>
    ''' Classe <see cref="ChildWindow"/> che controlla il processo di registrazione.
    ''' </summary>
    Partial Public Class LoginRegistrationWindow
        Inherits ChildWindow

        Private possiblyPendingOperations As IList(Of OperationBase) = New List(Of OperationBase)()

        ''' <summary>
        ''' Crea una nuova istanza di <see cref="LoginRegistrationWindow"/>.
        ''' </summary>
        Public Sub New()
            InitializeComponent()

            Me.registrationForm.SetParentWindow(Me)
            Me.loginForm.SetParentWindow(Me)

            AddHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            AddHandler Me.LayoutUpdated, AddressOf Me.UpdateTitle
        End Sub

        ''' <summary>
        ''' Inizializza l'oggetto <see cref="VisualStateManager"/> per questo componente attivandone lo stato "AtLogin"
        ''' </summary>
        Private Sub GoToInitialState(ByVal sender As Object, ByVal eventArgs As EventArgs)
            RemoveHandler Me.LayoutUpdated, AddressOf Me.GoToInitialState
            VisualStateManager.GoToState(Me, "AtLogin", False)
        End Sub

        ''' <summary>
        ''' Verifica che lo stato di visualizzazione e lo stato attivo siano corretti quando viene aperta la finestra.
        ''' </summary>
        Protected Overrides Sub OnOpened()
            MyBase.OnOpened()
            Me.NavigateToLogin()
        End Sub

        ''' <summary>
        ''' Aggiorna il titolo della finestra in base al pannello (registrazione o accesso) attualmente visualizzato.
        ''' </summary>
        Private Sub UpdateTitle(ByVal sender As Object, ByVal eventArgs As EventArgs)
            Me.Title = If(Me.registrationForm.Visibility = Visibility.Visible, _
                            ApplicationStrings.RegistrationWindowTitle, _
                            ApplicationStrings.LoginWindowTitle)
        End Sub

        ''' <summary>
        ''' Notifica alla finestra <see cref="LoginRegistrationWindow"/> che può essere chiusa solo se l'oggetto <paramref name="operation"/> è stato completato o può essere annullato.
        ''' </summary>
        ''' <param name="operation">Operazione in sospeso da monitorare</param>
        Public Sub AddPendingOperation(ByVal operation As OperationBase)
            Me.possiblyPendingOperations.Add(operation)
        End Sub

        ''' <summary>
        ''' Determina il passaggio allo stato "AtLogin" dell'oggetto <see cref="VisualStateManager"/>.
        ''' </summary>
        Public Overridable Sub NavigateToLogin()
            VisualStateManager.GoToState(Me, "AtLogin", True)
            Me.loginForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' Determina il passaggio allo stato "AtRegistration" dell'oggetto <see cref="VisualStateManager"/>.
        ''' </summary>
        Public Overridable Sub NavigateToRegistration()
            VisualStateManager.GoToState(Me, "AtRegistration", True)
            Me.registrationForm.SetInitialFocus()
        End Sub

        ''' <summary>
        ''' Impedisce la chiusura della finestra mentre è in corso un'operazione
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