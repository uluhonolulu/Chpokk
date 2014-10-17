Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' 控制何時要在 ErrorWindow 上顯示堆疊追蹤。
''' 預設為 <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' 只有在使用連接的偵錯工具執行，或在本機電腦上執行應用程式時才顯示堆疊追蹤。
    ''' 使用此工具取得您不想讓使用者看到的其他偵錯資訊。
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' 永遠顯示堆疊追蹤，即使偵錯
    ''' </summary>
    Always

    ''' <summary>
    ''' 永遠不顯示堆疊追蹤，即使偵錯
    ''' </summary>
    Never
End Enum

''' <summary>
''' 向使用者呈現錯誤的 <see cref="ChildWindow"/> 類別。
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' 建立新 <see cref="ErrorWindow"/> 執行個體。
    ''' </summary>
    ''' <param name="message">要顯示的錯誤訊息。</param>
    ''' <param name="errorDetails">有關此錯誤的其他資訊。</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "Factory 捷徑方法"
    ''' <summary>
    ''' 建立提供錯誤訊息的新錯誤視窗。
    ''' 如果應用程式是在偵錯模式或本機電腦上執行，將會顯示目前的堆疊追蹤。
    ''' </summary>
    ''' <param name="message">要顯示的訊息。</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 建立提供例外狀況的新錯誤視窗。
    ''' 如果應用程式是在偵錯模式或本機電腦上執行，將會顯示目前的堆疊追蹤。
    ''' 
    ''' 使用 <see cref="ConvertExceptionToMessage"/> 將例外狀況轉換成訊息。
    ''' </summary>
    ''' <param name="exception">要顯示的例外狀況。</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 建立提供例外狀況的新錯誤視窗。 
    ''' 使用 <see cref="ConvertExceptionToMessage"/> 將例外狀況轉換成訊息。
    ''' </summary>
    ''' <param name="exception">要顯示的例外狀況。</param>
    ''' <param name="policy">何時要顯示堆疊追蹤，請參閱 <see cref="StackTracePolicy"/>。</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' 說明巢狀例外狀況。
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' 建立提供錯誤訊息的新錯誤視窗。
    ''' </summary>
    ''' <param name="message">要顯示的訊息。</param>
    ''' <param name="policy">何時要顯示堆疊追蹤，請參閱 <see cref="StackTracePolicy"/>。</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "Factory 方法"
    ''' <summary>
    ''' 所有其他 Factory 方法將導致呼叫此方法
    ''' </summary>
    ''' <param name="message">要顯示哪一個訊息。</param>
    ''' <param name="stackTrace">關聯的堆疊追蹤。</param>
    ''' <param name="policy">在什麼情況下要把堆疊追蹤附加在訊息後面。</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "Factory Helper"
    ''' <summary>
    ''' 傳回是否要以所連接的偵錯工具執行，或是以 localhost 上裝載的伺服器執行。
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
    ''' 建立提供例外狀況的易懂訊息。 
    ''' 目前它只接受 Exception.Message 值。
    ''' 您可以修改這個方法將某些 Exception 類別以不同的方式處理。
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class