Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' Contrôle à quel moment la trace de la pile doit s'afficher sur l'ErrorWindow.
''' La valeur par défaut est <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' La trace de la pile est affichée uniquement lors d'une exécution avec un débogueur attaché ou lorsque l'application est exécutée sur l'ordinateur local.
    ''' Utilisez-la pour obtenir des informations de débogage supplémentaires que vous ne voulez pas afficher aux utilisateurs finals.
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' Toujours afficher la trace de la pile, même en cas de débogage
    ''' </summary>
    Always

    ''' <summary>
    ''' Ne jamais afficher la trace de la pile, même lors du débogage
    ''' </summary>
    Never
End Enum

''' <summary>
''' Classe <see cref="ChildWindow"/> qui présente les erreurs à l'utilisateur.
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' Crée une nouvelle instance <see cref="ErrorWindow"/>.
    ''' </summary>
    ''' <param name="message">Message d'erreur à afficher.</param>
    ''' <param name="errorDetails">Informations supplémentaires sur l'erreur.</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "Méthodes rapides de fabrique"
    ''' <summary>
    ''' Crée une nouvelle fenêtre d'erreurs avec un message d'erreur.
    ''' La trace de la pile actuelle s'affiche si l'application est en cours d'exécution lors du débogage ou sur un ordinateur local.
    ''' </summary>
    ''' <param name="message">Message à afficher.</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Crée une nouvelle fenêtre d'erreurs avec une exception.
    ''' La trace de la pile actuelle s'affiche si l'application est en cours d'exécution lors du débogage ou sur un ordinateur local.
    ''' 
    ''' L'exception est convertie en message à l'aide de <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">Exception à afficher.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Crée une nouvelle fenêtre d'erreurs avec une exception. 
    ''' L'exception est convertie en message à l'aide de <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">Exception à afficher.</param>
    ''' <param name="policy">Au moment d'afficher la trace de la pile, consulter <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' Compte pour les exceptions imbriquées.
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' Crée une nouvelle fenêtre d'erreurs avec un message d'erreur.
    ''' </summary>
    ''' <param name="message">Message à afficher.</param>
    ''' <param name="policy">Au moment d'afficher la trace de la pile, consulter <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "Méthodes de fabrique"
    ''' <summary>
    ''' Toutes les autres méthodes de fabrique entraîneront un appel à celle-ci
    ''' </summary>
    ''' <param name="message">Message à afficher.</param>
    ''' <param name="stackTrace">Trace de la pile associée.</param>
    ''' <param name="policy">Situations dans lesquelles la trace de la pile doit être ajoutée au message.</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "Programmes d'assistance de fabrique"
    ''' <summary>
    ''' Retourne une valeur indiquant si l'exécution s'effectue avec un débogueur attaché ou avec le serveur hébergé sur localhost.
    ''' </summary>
    Private Shared ReadOnly Property IsRunningUnderDebugOrLocalhost() As Boolean
        Get
            If Debugger.IsAttached Then
                Return True
            Else
                Dim hostUrl As String = Application.Current.Host.Source.Host
                Return hostUrl.Contains("::1") OrElse hostUrl.Contains("localhost") OrElse hostUrl.Contains("127.0.0.1")
            End If
        End Get
    End Property

    ''' <summary>
    ''' Crée un message convivial pour l'utilisateur avec une exception. 
    ''' Actuellement, il prend simplement la valeur Exception.Message.
    ''' Vous pouvez modifier cette méthode pour traiter certaines classes Exception différemment.
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class