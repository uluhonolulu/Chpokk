namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// ErrorWindow にスタック トレースを表示するタイミングを制御します。
    /// 既定値は <see cref="OnlyWhenDebuggingOrRunningLocally"/> です
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   スタック トレースが表示されるのは、デバッガーをアタッチして実行している場合、またはローカル コンピューターでアプリケーションを実行している場合です。
        ///   これを使用して、エンド ユーザーに表示しない追加のデバッグ情報を取得します。
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// デバッグの場合でも、スタック トレースを常に表示します
        /// </summary>
        Always,

        /// <summary>
        /// デバッグの場合でも、スタック トレースを表示しません
        /// </summary>
        Never
    }

    /// <summary>
    /// ユーザーにエラーを表示する <see cref="ChildWindow"/> クラスです。
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// 新しい <see cref="ErrorWindow"/> インスタンスを作成します。
        /// </summary>
        /// <param name="message">表示するエラー メッセージです。</param>
        /// <param name="errorDetails">エラーに関するその他の情報です。</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region ファクトリ ショートカット メソッド
        /// <summary>
        /// エラー メッセージが指定された新しいエラー ウィンドウを作成します。
        /// アプリケーションがデバッグ環境またはローカル コンピューターで実行されている場合、現在のスタック トレースが表示されます。
        /// </summary>
        /// <param name="message">表示するメッセージです。</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 例外が指定された新しいエラー ウィンドウを作成します。
        /// アプリケーションがデバッグ環境またはローカル コンピューターで実行されている場合、現在のスタック トレースが表示されます。
        /// 
        /// 例外は <see cref="ConvertExceptionToMessage"/> を使用してメッセージに変換されます。
        /// </summary>
        /// <param name="exception">表示する例外です。</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 例外が指定された新しいエラー ウィンドウを作成します。
        /// 例外は <see cref="ConvertExceptionToMessage"/> を使用してメッセージに変換されます。
        /// </summary>    
        /// <param name="exception">表示する例外です。</param>
        /// <param name="policy">スタック トレースを表示するタイミングについては、<see cref="StackTracePolicy"/> を参照してください。</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // 入れ子になった例外のアカウントです。
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// エラー メッセージが指定された新しいエラー ウィンドウを作成します。
        /// </summary>   
        /// <param name="message">表示するメッセージです。</param>
        /// <param name="policy">スタック トレースを表示するタイミングについては、<see cref="StackTracePolicy"/> を参照してください。</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region ファクトリ メソッド
        /// <summary>
        /// その他のファクトリ メソッドはすべて、このファクトリ メソッドの呼び出しになります。
        /// </summary>
        /// <param name="message">表示するメッセージです</param>
        /// <param name="stackTrace">関連付けられたスタック トレースです</param>
        /// <param name="policy">スタック トレースをメッセージに追加する必要がある状況です</param>
        private static void CreateNew(string message, string stackTrace, StackTracePolicy policy)
        {
            string errorDetails = string.Empty;

            if (policy == StackTracePolicy.Always || 
                policy == StackTracePolicy.OnlyWhenDebuggingOrRunningLocally && IsRunningUnderDebugOrLocalhost)
            {
                errorDetails = stackTrace ?? string.Empty;
            }

            ErrorWindow window = new ErrorWindow(message, errorDetails);
            window.Show();
        }
        #endregion

        #region ファクトリ ヘルパー
        /// <summary>
        /// デバッガーをアタッチして実行しているか、localhost でホストされているサーバーで実行されているかを返します。
        /// </summary>
        private static bool IsRunningUnderDebugOrLocalhost
        {
            get
            {
                if (Debugger.IsAttached)
                {
                    return true;
                }
                else
                {                    
                    string hostUrl = Application.Current.Host.Source.Host;
                    return hostUrl.Contains("::1") || hostUrl.Contains("localhost") || hostUrl.Contains("127.0.0.1");                       
                }
            }
        }

        /// <summary>
        /// 例外が指定されたユーザー フレンドリなメッセージを作成します。
        /// 現在、このメソッドは Exception.Message 値を返します。
        /// 特定の Exception クラスを異なる方法で処理するように、このメソッドを変更できます。
        /// </summary>
        /// <param name="e">変換する例外です。</param>
        private static string ConvertExceptionToMessage(Exception e)
        {
            return e.Message;
        }
        #endregion

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}