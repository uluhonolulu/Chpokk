namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// 스택 추적이 ErrorWindow에 표시되는 시간을 제어합니다.
    /// 기본값은 <see cref="OnlyWhenDebuggingOrRunningLocally"/>입니다.
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   스택 추적은 디버거를 연결한 상태로 실행하거나 로컬 컴퓨터에서 응용 프로그램을 실행하는 경우에 표시됩니다.
        ///   최종 사용자에게 표시하지 않을 추가 디버그 정보를 보려면 사용합니다.
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// 디버깅 중에도 스택 추적 항상 표시
        /// </summary>
        Always,

        /// <summary>
        /// 디버깅 중에도 스택 추적 표시 안 함
        /// </summary>
        Never
    }

    /// <summary>
    /// 사용자에게 오류를 표시하는 <see cref="ChildWindow"/> 클래스입니다.
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// 새 <see cref="ErrorWindow"/> 인스턴스를 만듭니다.
        /// </summary>
        /// <param name="message">표시할 오류 메시지입니다.</param>
        /// <param name="errorDetails">오류에 대한 추가 정보입니다.</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region 팩터리 바로 가기 메서드
        /// <summary>
        /// 오류 메시지가 있을 경우 새 오류 창을 만듭니다.
        /// 디버그 중에 또는 로컬 컴퓨터에서 응용 프로그램을 실행 중인 경우 현재 스택 추적이 표시됩니다.
        /// </summary>
        /// <param name="message">표시할 메시지입니다.</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 예외가 있을 경우 새 오류 창을 만듭니다.
        /// 디버그 중에 또는 로컬 컴퓨터에서 응용 프로그램을 실행 중인 경우 현재 스택 추적이 표시됩니다.
        /// 
        /// <see cref="ConvertExceptionToMessage"/>를 사용하여 예외를 메시지로 변환합니다.
        /// </summary>
        /// <param name="exception">표시할 예외입니다.</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// 예외가 있을 경우 새 오류 창을 만듭니다.
        /// <see cref="ConvertExceptionToMessage"/>를 사용하여 예외를 메시지로 변환합니다.
        /// </summary>    
        /// <param name="exception">표시할 예외입니다.</param>
        /// <param name="policy">스택 추적을 표시할 경우 <see cref="StackTracePolicy"/>를 참조하십시오.</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // 중첩 예외 고려
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// 오류 메시지가 있을 경우 새 오류 창을 만듭니다.
        /// </summary>   
        /// <param name="message">표시할 메시지입니다.</param>
        /// <param name="policy">스택 추적을 표시할 경우 <see cref="StackTracePolicy"/>를 참조하십시오.</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region 팩터리 메서드
        /// <summary>
        /// 모든 다른 팩터리 메서드는 이 메서드를 호출합니다.
        /// </summary>
        /// <param name="message">표시할 메시지입니다.</param>
        /// <param name="stackTrace">연결된 스택 추적입니다.</param>
        /// <param name="policy">스택 추적을 메시지에 추가해야 하는 경우입니다.</param>
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

        #region 팩터리 도우미
        /// <summary>
        /// 디버거를 연결한 상태로 실행 중인지, 서버를 localhost에서 호스팅하는 상태로 실행 중인지 여부를 반환합니다.
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
        /// Exception이 있을 경우 사용자에게 친숙한 메시지를 만듭니다. 
        /// 현재 이 메서드는 Exception.Message 값을 반환합니다. 
        /// 특정 Exception 클래스를 다르게 처리하도록 이 메서드를 수정할 수 있습니다.
        /// </summary>
        /// <param name="e">변환할 예외입니다.</param>
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