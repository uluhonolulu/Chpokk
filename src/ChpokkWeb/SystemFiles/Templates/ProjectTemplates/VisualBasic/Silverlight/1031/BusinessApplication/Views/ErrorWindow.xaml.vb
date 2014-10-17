Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' Steuert, wann eine Stapelüberwachung in ErrorWindow angezeigt werden soll.
''' Der Standardwert lautet <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' "Stapelüberwachung" wird nur angezeigt, wenn die Anwendung mit einem Debugger verbunden ist oder auf dem lokalen Computer ausgeführt wird.
    ''' Verwenden Sie dies zum Anzeigen zusätzlicher Debuginformationen, die dem Endbenutzer nicht angezeigt werden sollen.
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' Die Stapelüberwachung immer anzeigen, auch beim Debugging
    ''' </summary>
    Always

    ''' <summary>
    ''' Die Stapelüberwachung niemals anzeigen, auch nicht beim Debugging
    ''' </summary>
    Never
End Enum

''' <summary>
''' <see cref="ChildWindow"/> Klasse, die dem Benutzer Fehler anzeigt.
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' Erstellt eine neue Instanz von <see cref="ErrorWindow"/>.
    ''' </summary>
    ''' <param name="message">Die Fehlermeldung, die angezeigt wird.</param>
    ''' <param name="errorDetails">Zusätzliche Fehlerinformationen.</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "Factory Shortcut-Methoden"
    ''' <summary>
    ''' Zeigt ein Fehlerfenster an, falls eine Fehlermeldung vorhanden ist.
    ''' Aktuelle Stapelüberwachung wird angezeigt, wenn die Anwendung im Debugmodus oder auf dem lokalen Computer ausgeführt wird.
    ''' </summary>
    ''' <param name="message">Die Meldung, die angezeigt wird.</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Erstellt ein neues Fehlerfenster, falls eine Ausnahme vorhanden ist.
    ''' Aktuelle Stapelüberwachung wird angezeigt, wenn die Anwendung im Debugmodus oder auf dem lokalen Computer ausgeführt wird.
    ''' 
    ''' Die Ausnahme wird mithilfe von <see cref="ConvertExceptionToMessage"/> in eine Nachricht umgewandelt.
    ''' </summary>
    ''' <param name="exception">Die Ausnahme, die angezeigt wird.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Erstellt ein neues Fehlerfenster, falls eine Ausnahme vorhanden ist. 
    ''' Die Ausnahme wird mithilfe von <see cref="ConvertExceptionToMessage"/> in eine Nachricht umgewandelt.
    ''' </summary>
    ''' <param name="exception">Die Ausnahme, die angezeigt wird.</param>
    ''' <param name="policy">Siehe <see cref="StackTracePolicy"/>, wann die Stapelüberwachung angezeigt wird.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' Konto für geschachtelte Ausnahmen.
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' Zeigt ein Fehlerfenster an, falls eine Fehlermeldung vorhanden ist.
    ''' </summary>
    ''' <param name="message">Die Meldung, die angezeigt wird.</param>
    ''' <param name="policy">Siehe <see cref="StackTracePolicy"/>, wann die Stapelüberwachung angezeigt wird.</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "Factory-Methoden"
    ''' <summary>
    ''' Alle anderen Factory-Methoden haben einen Aufruf dieser Methode zur Folge
    ''' </summary>
    ''' <param name="message">Die Fehlermeldung, die angezeigt wird.</param>
    ''' <param name="stackTrace">Die zugeordnete Stapelüberwachung.</param>
    ''' <param name="policy">In welchen Situationen die Stapelüberwachung an die Nachricht angehängt werden soll.</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "Factory-Hilfsprogramme"
    ''' <summary>
    ''' Gibt zurück, ob eine Verbindung zu einem Debugger besteht oder der Server auf localhost gehostet ist.
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
    ''' Erstellt im Falle einer Ausnahme eine benutzerfreundliche Nachricht. 
    ''' Aktuell wird einfach der Wert von Exception.Message übernommen.
    ''' Diese Methode kann geändert werden, um bestimmte "Exception"-Klassen unterschiedlich zu behandeln.
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class