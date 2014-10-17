namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// 控制何時要在 ErrorWindow 中顯示堆疊追蹤。
    /// 預設為 <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   在使用附加偵錯工具的情況下執行或在本機電腦上執行應用程式時，堆疊追蹤就會顯示。
        ///   使用此工具取得您不想讓使用者看到的其他偵錯資訊。
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// 永遠顯示堆疊追蹤，即使偵錯
        /// </summary>
        Always,

        /// <summary>
        /// 永遠不顯示堆疊追蹤，即使偵錯
        /// </summary>
        Never
    }

    /// <summary>
    /// 向使用者顯示錯誤的 <see cref="ChildWindow"/> 類別。
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// 建立新 <see cref="ErrorWindow"/> 執行個體。
        /// </summary>
        /// <param name="message">要顯示的錯誤訊息。</param>
        /// <param name="errorDetails">有關此錯誤的其他資訊。</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region Factory 捷徑方法
        /// <summary>
        /// 建立提供錯誤訊息的新錯誤視窗。
        /// 如果應用程式是在偵錯模式或本機電腦上執行，將會顯示目前的堆疊追蹤。
        /// </summary>
        /// <param name="message">要顯示的訊息。</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 建立提供例外狀況的新錯誤視窗。
        /// 如果應用程式是在偵錯模式或本機電腦上執行，將會顯示目前的堆疊追蹤。
        /// 
        /// 使用 <see cref="ConvertExceptionToMessage"/> 將例外狀況轉換成訊息。
        /// </summary>
        /// <param name="exception">要顯示的例外狀況。</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 建立提供例外狀況的新錯誤視窗。
        /// 使用 <see cref="ConvertExceptionToMessage"/> 將例外狀況轉換成訊息。
        /// </summary>    
        /// <param name="exception">要顯示的例外狀況。</param>
        /// <param name="policy">何時要顯示堆疊追蹤，請參閱 <see cref="StackTracePolicy"/>。</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // 說明巢狀例外狀況
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// 建立提供錯誤訊息的新錯誤視窗。
        /// </summary>   
        /// <param name="message">要顯示的訊息。</param>
        /// <param name="policy">何時要顯示堆疊追蹤，請參閱 <see cref="StackTracePolicy"/>。</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region Factory 方法
        /// <summary>
        /// 所有其他 Factory 方法將導致呼叫此方法。
        /// </summary>
        /// <param name="message">要顯示的訊息</param>
        /// <param name="stackTrace">關聯的堆疊追蹤</param>
        /// <param name="policy">應該把堆疊追蹤附加在訊息後面的情況</param>
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

        #region Factory Helper
        /// <summary>
        /// 傳回是否要以所連接的偵錯工具執行，或是以 localhost 上裝載的伺服器執行。
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
        /// 建立提供例外狀況的易懂訊息。 
        /// 目前此方法會傳回 Exception.Message 值。 
        /// 您可以修改這個方法將某些 Exception 類別以不同的方式處理。
        /// </summary>
        /// <param name="e">要轉換的例外狀況。</param>
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