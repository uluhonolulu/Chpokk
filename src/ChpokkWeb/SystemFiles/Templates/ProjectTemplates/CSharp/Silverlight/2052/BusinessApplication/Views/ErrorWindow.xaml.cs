namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// 控制应在 ErrorWindow 中显示堆栈跟踪的时间。
    /// 默认为 <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   仅当在附加了调试器的情况下运行，或在本地计算机上运行应用程序时，才显示堆栈跟踪。
        ///   使用此功能可获取您不希望最终用户看到的其他调试信息。
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// 始终显示堆栈跟踪，即使正在调试
        /// </summary>
        Always,

        /// <summary>
        /// 从不显示堆栈跟踪，即使正在调试
        /// </summary>
        Never
    }

    /// <summary>
    /// 向用户提供错误的 <see cref="ChildWindow"/> 类。
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// 创建新 <see cref="ErrorWindow"/> 实例。
        /// </summary>
        /// <param name="message">要显示的错误消息。</param>
        /// <param name="errorDetails">有关错误的额外信息。</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region 工厂快捷方法
        /// <summary>
        /// 在出现错误消息时创建新错误窗口。
        /// 如果应用程序在调试模式下或在本地计算机上运行，则会显示当前堆栈跟踪。
        /// </summary>
        /// <param name="message">要显示的消息。</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 在出现异常时创建新错误窗口。
        /// 如果应用程序在调试模式下或在本地计算机上运行，则会显示当前堆栈跟踪。
        /// 
        /// 使用 <see cref="ConvertExceptionToMessage"/> 将异常转换为消息。
        /// </summary>
        /// <param name="exception">要显示的异常。</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 在出现异常时创建新错误窗口。
        /// 使用 <see cref="ConvertExceptionToMessage"/> 将异常转换为消息。
        /// </summary>    
        /// <param name="exception">要显示的异常。</param>
        /// <param name="policy">显示堆栈跟踪的时间，请参见 <see cref="StackTracePolicy"/>。</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // 解释嵌套异常。
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// 在出现错误消息时创建新错误窗口。
        /// </summary>   
        /// <param name="message">要显示的消息。</param>
        /// <param name="policy">显示堆栈跟踪的时间，请参见 <see cref="StackTracePolicy"/>。</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region 工厂方法
        /// <summary>
        /// 所有其他工厂方法都会导致对此方法的调用。
        /// </summary>
        /// <param name="message">要显示的消息</param>
        /// <param name="stackTrace">关联的堆栈跟踪</param>
        /// <param name="policy">将堆栈跟踪追加到消息的条件。</param>
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

        #region 工厂帮助程序
        /// <summary>
        /// 返回是在附加了调试器的情况下运行，还是在 localhost 承载的服务器上运行。
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
        /// 在出现异常时创建对用户友好的消息。 
        /// 当前此方法返回 Exception.Message 值。
        /// 可以修改此方法以按不同方式处理特定 Exception 类。
        /// </summary>
        /// <param name="e">要转换的异常。</param>
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