Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' 控制应在 ErrorWindow 上显示堆栈跟踪的时间。
''' 默认为 <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' 仅当在附加了调试器的情况下运行，或在本地计算机上运行应用程序时，才显示堆栈跟踪。
    ''' 使用此功能可获取您不希望最终用户看到的其他调试信息。
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' 始终显示堆栈跟踪，即使正在调试
    ''' </summary>
    Always

    ''' <summary>
    ''' 从不显示堆栈跟踪，即使正在调试
    ''' </summary>
    Never
End Enum

''' <summary>
''' 向用户提供错误的 <see cref="ChildWindow"/> 类。
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' 创建新 <see cref="ErrorWindow"/> 实例。
    ''' </summary>
    ''' <param name="message">要显示的错误消息。</param>
    ''' <param name="errorDetails">有关错误的额外信息。</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "工厂快捷方法"
    ''' <summary>
    ''' 在出现错误消息时创建新错误窗口。
    ''' 如果应用程序在调试模式下或在本地计算机上运行，则会显示当前堆栈跟踪。
    ''' </summary>
    ''' <param name="message">要显示的消息。</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 在出现异常时创建新错误窗口。
    ''' 如果应用程序在调试模式下或在本地计算机上运行，则会显示当前堆栈跟踪。
    ''' 
    ''' 使用 <see cref="ConvertExceptionToMessage"/> 将异常转换为消息。
    ''' </summary>
    ''' <param name="exception">要显示的异常。</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 在出现异常时创建新错误窗口。 
    ''' 使用 <see cref="ConvertExceptionToMessage"/> 将异常转换为消息。
    ''' </summary>
    ''' <param name="exception">要显示的异常。</param>
    ''' <param name="policy">显示堆栈跟踪的时间，请参见 <see cref="StackTracePolicy"/>。</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' 解释嵌套异常。
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' 在出现错误消息时创建新错误窗口。
    ''' </summary>
    ''' <param name="message">要显示的消息。</param>
    ''' <param name="policy">显示堆栈跟踪的时间，请参见 <see cref="StackTracePolicy"/>。</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "工厂方法"
    ''' <summary>
    ''' 所有其他工厂方法都会导致对此方法的调用
    ''' </summary>
    ''' <param name="message">要显示的消息。</param>
    ''' <param name="stackTrace">关联的堆栈跟踪。</param>
    ''' <param name="policy">将堆栈跟踪追加到消息的条件。</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "工厂帮助程序"
    ''' <summary>
    ''' 返回是在附加了调试器的情况下运行，还是在 localhost 承载的服务器上运行。
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
    ''' 在出现异常时创建对用户友好的消息。 
    ''' 目前此方法只是采用 Exception.Message 值而已。
    ''' 可以修改此方法以按不同方式处理特定 Exception 类。
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class