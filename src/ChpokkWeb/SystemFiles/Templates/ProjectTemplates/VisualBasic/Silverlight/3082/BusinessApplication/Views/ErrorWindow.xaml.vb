Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' Controla cuándo tiene que mostrarse un seguimiento de la pila en la ErrorWindow.
''' Se establece de forma predeterminada en <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' El seguimiento de la pila se muestra únicamente al ejecutar con un depurador adjunto o al ejecutar la aplicación en la máquina local.
    ''' Utilice esto para obtener información adicional de depuración que no desea que vean los usuarios finales.
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' Mostrar siempre el seguimiento de la pila, aunque se esté depurando
    ''' </summary>
    Always

    ''' <summary>
    ''' No mostrar nunca el seguimiento de la pila, ni al depurar
    ''' </summary>
    Never
End Enum

''' <summary>
''' Clase <see cref="ChildWindow"/> que presenta errores al usuario.
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' Crea una nueva instancia de <see cref="ErrorWindow"/>.
    ''' </summary>
    ''' <param name="message">Mensaje de error que se va a mostrar.</param>
    ''' <param name="errorDetails">Información adicional sobre el error.</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "Métodos abreviados de fábrica"
    ''' <summary>
    ''' Crea una nueva ventana de error en función de un mensaje de error.
    ''' Si la aplicación se está ejecutando en modo depuración o en la máquina local, se mostrará el seguimiento de la pila actual.
    ''' </summary>
    ''' <param name="message">Mensaje que se va a mostrar.</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Crea una nueva ventana de error en función de una exception.
    ''' Si la aplicación se está ejecutando en modo depuración o en la máquina local, se mostrará el seguimiento de la pila actual.
    ''' 
    ''' La exception se convierte en un mensaje mediante <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">Es la exception que se va a mostrar.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Crea una nueva ventana de error en función de una exception. 
    ''' La exception se convierte en un mensaje mediante <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">Es la exception que se va a mostrar.</param>
    ''' <param name="policy">Si va a mostrar el seguimiento de la pila, consulte <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' Cuenta para exceptions anidadas.
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' Crea una nueva ventana de error en función de un mensaje de error.
    ''' </summary>
    ''' <param name="message">Mensaje que se va a mostrar.</param>
    ''' <param name="policy">Si va a mostrar el seguimiento de la pila, consulte <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "Métodos de fábrica"
    ''' <summary>
    ''' Todos los demás métodos de fábrica darán lugar a una llamada a éste
    ''' </summary>
    ''' <param name="message">Qué mensaje se va a mostrar.</param>
    ''' <param name="stackTrace">Seguimiento de la pila asociado.</param>
    ''' <param name="policy">En qué situaciones se debe anexar el seguimiento de la pila al mensaje.</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "Aplicaciones auxiliares de fábrica"
    ''' <summary>
    ''' Devuelve si se está ejecutando con un depurador adjunto o con el servidor hospedado en localhost.
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
    ''' Crea un mensaje fácil de usar en función de una Exception. 
    ''' En este momento esto simplemente toma el valor Exception.Message.
    ''' Se puede modificar este método para tratar ciertas clases Exception de forma distinta.
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class