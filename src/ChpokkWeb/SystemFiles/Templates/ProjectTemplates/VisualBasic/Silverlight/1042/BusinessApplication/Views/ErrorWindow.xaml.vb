Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' 스택 추적이 ErrorWindow에 표시되는 시간을 제어합니다.
''' 기본값은 <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/>입니다.
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' 스택 추적은 디버거를 연결한 상태로 실행하거나 로컬 컴퓨터에서 응용 프로그램을 실행하는 경우에만 표시됩니다.
    ''' 최종 사용자에게 표시하지 않을 추가 디버그 정보를 보려면 사용합니다.
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' 디버깅 중에도 스택 추적 항상 표시
    ''' </summary>
    Always

    ''' <summary>
    ''' 디버깅 중에도 스택 추적 표시 안 함
    ''' </summary>
    Never
End Enum

''' <summary>
''' 사용자에게 오류를 표시하는 <see cref="ChildWindow"/> 클래스입니다.
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' 새 <see cref="ErrorWindow"/> 인스턴스를 만듭니다.
    ''' </summary>
    ''' <param name="message">표시할 오류 메시지입니다.</param>
    ''' <param name="errorDetails">오류에 대한 추가 정보입니다.</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "팩터리 바로 가기 메서드"
    ''' <summary>
    ''' 오류 메시지가 있을 경우 새 오류 창을 만듭니다.
    ''' 디버그 중에 또는 로컬 컴퓨터에서 응용 프로그램을 실행 중인 경우 현재 스택 추적이 표시됩니다.
    ''' </summary>
    ''' <param name="message">표시할 메시지입니다.</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 예외가 있을 경우 새 오류 창을 만듭니다.
    ''' 디버그 중에 또는 로컬 컴퓨터에서 응용 프로그램을 실행 중인 경우 현재 스택 추적이 표시됩니다.
    ''' 
    ''' <see cref="ConvertExceptionToMessage"/>를 사용하여 예외를 메시지로 변환합니다.
    ''' </summary>
    ''' <param name="exception">표시할 예외입니다.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 예외가 있을 경우 새 오류 창을 만듭니다. 
    ''' <see cref="ConvertExceptionToMessage"/>를 사용하여 예외를 메시지로 변환합니다.
    ''' </summary>
    ''' <param name="exception">표시할 예외입니다.</param>
    ''' <param name="policy">스택 추적을 표시할 경우 <see cref="StackTracePolicy"/>를 참조하십시오.</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' 중첩 예외를 고려합니다.
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' 오류 메시지가 있을 경우 새 오류 창을 만듭니다.
    ''' </summary>
    ''' <param name="message">표시할 메시지입니다.</param>
    ''' <param name="policy">스택 추적을 표시할 경우 <see cref="StackTracePolicy"/>를 참조하십시오.</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "팩터리 메서드"
    ''' <summary>
    ''' 모든 다른 팩터리 메서드는 이 메서드를 호출합니다.
    ''' </summary>
    ''' <param name="message">표시할 메시지입니다.</param>
    ''' <param name="stackTrace">연결된 스택 추적입니다.</param>
    ''' <param name="policy">스택 추적을 메시지에 추가해야 하는 경우입니다.</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "팩터리 도우미"
    ''' <summary>
    ''' 디버거를 연결한 상태로 실행 중인지, 서버를 localhost에서 호스팅하는 상태로 실행 중인지 여부를 반환합니다.
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
    ''' Exception이 있을 경우 사용자에게 친숙한 메시지를 만듭니다. 
    ''' 현재는 Exception.Message 값을 가져옵니다.
    ''' 특정 Exception 클래스를 다르게 처리하도록 이 메서드를 수정할 수 있습니다.
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class