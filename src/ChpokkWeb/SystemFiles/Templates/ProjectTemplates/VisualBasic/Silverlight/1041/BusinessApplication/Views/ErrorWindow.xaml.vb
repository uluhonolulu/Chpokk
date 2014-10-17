Imports System
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Controls
Imports System.Net
Imports Microsoft.VisualBasic.ControlChars

''' <summary>
''' ErrorWindow にスタック トレースを表示するタイミングを制御します。
''' 既定値は <see cref="StackTracePolicy.OnlyWhenDebuggingOrRunningLocally"/> です
''' </summary>
Public Enum StackTracePolicy
    ''' <summary>
    ''' スタック トレースが表示されるのは、デバッガーをアタッチして実行している場合、またはローカル コンピューターでアプリケーションを実行している場合のみです。
    ''' これを使用して、エンド ユーザーに表示しない追加のデバッグ情報を取得します。
    ''' </summary>
    OnlyWhenDebuggingOrRunningLocally

    ''' <summary>
    ''' デバッグの場合でも、スタック トレースを常に表示します
    ''' </summary>
    Always

    ''' <summary>
    ''' デバッグの場合でも、スタック トレースを表示しません
    ''' </summary>
    Never
End Enum

''' <summary>
''' ユーザーにエラーを表示する <see cref="ChildWindow"/> クラスです。
''' </summary>
Partial Public Class ErrorWindow
    Inherits ChildWindow

    ''' <summary>
    ''' 新しい <see cref="ErrorWindow"/> インスタンスを作成します。
    ''' </summary>
    ''' <param name="message">表示するエラー メッセージです。</param>
    ''' <param name="errorDetails">エラーに関するその他の情報です。</param>
    Protected Sub New(ByVal message As String, ByVal errorDetails As String)
        InitializeComponent()
        IntroductoryText.Text = message
        ErrorTextBox.Text = errorDetails
    End Sub

#Region "ファクトリ ショートカット メソッド"
    ''' <summary>
    ''' エラー メッセージが指定された新しいエラー ウィンドウを作成します。
    ''' アプリケーションがデバッグ環境またはローカル コンピューターで実行されている場合、現在のスタック トレースが表示されます。
    ''' </summary>
    ''' <param name="message">表示するメッセージです。</param>
    Public Shared Sub CreateNew(ByVal message As String)
        CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 例外が指定された新しいエラー ウィンドウを作成します。
    ''' アプリケーションがデバッグ環境またはローカル コンピューターで実行されている場合、現在のスタック トレースが表示されます。
    ''' 
    ''' 例外は <see cref="ConvertExceptionToMessage"/> を使用してメッセージに変換されます。
    ''' </summary>
    ''' <param name="exception">表示する例外です。</param>
    Public Shared Sub CreateNew(ByVal exception As Exception)
        CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally)
    End Sub

    ''' <summary>
    ''' 例外が指定された新しいエラー ウィンドウを作成します。
    ''' 例外は <see cref="ConvertExceptionToMessage"/> を使用してメッセージに変換されます。
    ''' </summary>
    ''' <param name="exception">表示する例外です。</param>
    ''' <param name="policy">スタック トレースを表示するタイミングについては、<see cref="StackTracePolicy"/> を参照してください。</param>
    Public Shared Sub CreateNew(ByVal exception As Exception, ByVal policy As StackTracePolicy)
        If exception Is Nothing Then
            Throw New ArgumentNullException("exception")
        End If

        Dim fullStackTrace As String = exception.StackTrace

        ' 入れ子になった例外のアカウントです。
        Dim innerException As Exception = exception.InnerException

        While innerException IsNot Nothing
            fullStackTrace += (CrLf & String.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) & CrLf & CrLf) + innerException.StackTrace
            innerException = innerException.InnerException
        End While

        CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy)
    End Sub

    ''' <summary>
    ''' エラー メッセージが指定された新しいエラー ウィンドウを作成します。
    ''' </summary>
    ''' <param name="message">表示するメッセージです。</param>
    ''' <param name="policy">スタック トレースを表示するタイミングについては、<see cref="StackTracePolicy"/> を参照してください。</param>
    Public Shared Sub CreateNew(ByVal message As String, ByVal policy As StackTracePolicy)
        CreateNew(message, New StackTrace().ToString(), policy)
    End Sub
#End Region

#Region "ファクトリ メソッド"
    ''' <summary>
    ''' その他のファクトリ メソッドはすべて、このファクトリ メソッドの呼び出しになります
    ''' </summary>
    ''' <param name="message">表示するメッセージです。</param>
    ''' <param name="stackTrace">関連付けられたスタック トレースです。</param>
    ''' <param name="policy">スタック トレースをメッセージに追加する必要がある状況です。</param>
    Private Shared Sub CreateNew(ByVal message As String, ByVal stackTrace As String, ByVal policy As StackTracePolicy)
        Dim errorDetails As String = String.Empty

        If policy = StackTracePolicy.Always OrElse policy = StackTracePolicy.OnlyWhenDebuggingOrRunningLocally AndAlso IsRunningUnderDebugOrLocalhost Then
            errorDetails = If(stackTrace Is Nothing, String.Empty, stackTrace)
        End If

        Dim window As New ErrorWindow(message, errorDetails)
        window.Show()
    End Sub
#End Region

#Region "ファクトリ ヘルパー"
    ''' <summary>
    ''' デバッガーをアタッチして実行しているか、localhost でホストされているサーバーで実行されているかを返します。
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
    ''' 例外が指定されたユーザー フレンドリなメッセージを作成します。
    ''' 現在は、これにより Exception.Message 値が取得されます。
    ''' 特定の Exception クラスを異なる方法で処理するように、このメソッドを変更できます。
    ''' </summary>
    Private Shared Function ConvertExceptionToMessage(ByVal e As Exception) As String
        Return e.Message
    End Function
#End Region

    Private Sub OKButton_Click(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Me.DialogResult = True
    End Sub
End Class