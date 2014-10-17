Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' Controlla quando visualizzare una traccia dello stack in ErrorWindow.
''' Impostazione predefinita: <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' La traccia dello stack viene visualizzata solo se l'applicazione viene eseguita con un debugger associato o nel computer locale.
    ''' Da utilizzare per ottenere ulteriori informazioni sul debug che non si desidera vengano visualizzate dagli utenti finali.
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' Mostrare sempre la traccia dello stack, anche durante il debug
    ''' </summary>
    Always

    ''' <summary>
    ''' Non mostrare la traccia dello stack, neanche durante il debug
    ''' </summary>
    Never
End Enum

''' <summary>
''' Classe <see cref="ChildWindow"/> che mostra gli errori all'utente.
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' Crea una nuova istanza di <see cref="ErrorWindow"/>.
    ''' </summary>
    ''' <param name="message">Messaggio di errore da visualizzare.</param>
    ''' <param name="errorDetails">Informazioni aggiuntive sull'errore.</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "Metodi rapidi factory"
    ''' <summary>
    ''' Crea una nuova finestra di errore in base a un messaggio di errore.
    ''' La traccia dello stack corrente verrà visualizzata se l'applicazione viene eseguita durante il debug o nel computer locale.
    ''' </summary>
    ''' <param name="message">Messaggio da visualizzare.</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Crea una nuova finestra di errore in base a un'eccezione.
    ''' La traccia dello stack corrente verrà visualizzata se l'applicazione viene eseguita durante il debug o nel computer locale.
    ''' 
    ''' L'eccezione viene convertita in un messaggio mediante <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">L'eccezione da visualizzare.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Crea una nuova finestra di errore in base a un'eccezione. 
    ''' L'eccezione viene convertita in un messaggio mediante <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">L'eccezione da visualizzare.</param>
    ''' <param name="policy">Per stabilire quando visualizzare la traccia dello stack, vedere <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' Account per eccezioni annidate.
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' Crea una nuova finestra di errore in base a un messaggio di errore.
    ''' </summary>
    ''' <param name="message">Messaggio da visualizzare.</param>
    ''' <param name="policy">Per stabilire quando visualizzare la traccia dello stack, vedere <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "Metodi factory"
    ''' <summary>
    ''' Tutti gli altri metodi factory determineranno una chiamata a questo metodo
    ''' </summary>
    ''' <param name="message">Messaggio da visualizzare.</param>
    ''' <param name="stackTrace">Traccia dello stack associata.</param>
    ''' <param name="policy">Situazioni in cui aggiungere la traccia dello stack al messaggio.</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "Supporti factory"
    ''' <summary>
    ''' Restituisce un valore che indica se eseguire l'applicazione con un debugger associato o con il server ospitato su localhost.
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
    ''' Crea un messaggio descrittivo in base a un'eccezione. 
    ''' Attualmente accetta semplicemente il valore Exception.Message.
    ''' È possibile modificare questo metodo in modo da considerare alcune classi Exception in modo diverso.
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class