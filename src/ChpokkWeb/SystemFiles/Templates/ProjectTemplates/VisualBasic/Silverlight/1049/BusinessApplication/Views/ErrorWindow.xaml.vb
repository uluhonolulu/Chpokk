Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' Определяет, будет ли отображаться в окне ErrorWindow трассировка стека.
''' Значение по умолчанию <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' Трассировка стека отображается только в режиме отладки либо при запуске приложения на локальном компьютере.
    ''' Служит для получения дополнительных отладочных данных, которые нежелательно показывать конечным пользователям.
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' Всегда отображать трассировку стека, даже при отладке
    ''' </summary>
    Always

    ''' <summary>
    ''' Никогда не отображать трассировку стека, даже при отладке
    ''' </summary>
    Never
End Enum

''' <summary>
''' Класс <see cref="ChildWindow"/>, представляющий ошибки для пользователя.
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' Создает новый экземпляр класса <see cref="ErrorWindow"/>.
    ''' </summary>
    ''' <param name="message">Сообщение об ошибке для отображения.</param>
    ''' <param name="errorDetails">Дополнительные сведения об ошибке.</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "Методы быстрого вызова фабрики"
    ''' <summary>
    ''' Создает новое окно сообщения об ошибке, содержащее указанное сообщение.
    ''' Отображает текущую трассировку стека, если приложение работает в режиме отладки или на локальном компьютере.
    ''' </summary>
    ''' <param name="message">Сообщение для отображения.</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Создает новое окно сообщения об ошибке, содержащее исключение.
    ''' Отображает текущую трассировку стека, если приложение работает в режиме отладки или на локальном компьютере.
    ''' 
    ''' Исключение (exception) преобразуется в сообщение с помощью метода <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">Исключение (exception) для отображения.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' Создает новое окно сообщения об ошибке, содержащее исключение.
    ''' Исключение (exception) преобразуется в сообщение с помощью метода <see cref="ConvertExceptionToMessage"/>.
    ''' </summary>
    ''' <param name="exception">Исключение (exception) для отображения.</param>
    ''' <param name="policy">Если нужно отобразить трассировку стека, см. раздел <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' Учитывать вложенные исключения (exceptions).
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' Создает новое окно сообщения об ошибке, содержащее указанное сообщение.
    ''' </summary>
    ''' <param name="message">Сообщение для отображения.</param>
    ''' <param name="policy">Если нужно отобразить трассировку стека, см. раздел <see cref="StackTracePolicy"/>.</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "Фабричные методы"
    ''' <summary>
    ''' Все остальные фабричные методы приводят к вызову этого
    ''' </summary>
    ''' <param name="message">Сообщение для отображения.</param>
    ''' <param name="stackTrace">Связанная трассировка стека.</param>
    ''' <param name="policy">Случаи, в которых трассировка стека должна быть добавлена к сообщению.</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "Помощники фабрики"
    ''' <summary>
    ''' Возвращает значение, определяющее, что приложение запущено в режиме отладки или на локальном компьютере (localhost).
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
    ''' Создает на основе исключения (Exception) понятное для пользователя сообщение. 
    ''' В настоящее время просто берется значение свойства Exception.Message.
    ''' Этот метод можно изменить так, чтобы некоторые из классов исключений (Exception) трактовались по-другому.
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class