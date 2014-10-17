namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// Определяет, будет ли отображаться в окне ErrorWindow трассировка стека.
    /// Значение по умолчанию <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   Трассировка стека отображается только в режиме отладки либо при запуске приложения на локальном компьютере.
        ///   Служит для получения дополнительных отладочных данных, которые нежелательно показывать конечным пользователям.
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// Всегда отображать трассировку стека, даже при отладке
        /// </summary>
        Always,

        /// <summary>
        /// Никогда не отображать трассировку стека, даже при отладке
        /// </summary>
        Never
    }

    /// <summary>
    /// Класс <see cref="ChildWindow"/>, представляющий ошибки для пользователя.
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// Создает новый экземпляр класса <see cref="ErrorWindow"/>.
        /// </summary>
        /// <param name="message">Сообщение об ошибке для отображения.</param>
        /// <param name="errorDetails">Дополнительные сведения об ошибке.</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region Методы быстрого вызова фабрики
        /// <summary>
        /// Создает новое окно сообщения об ошибке, содержащее указанное сообщение.
        /// Отображает текущую трассировку стека, если приложение работает в режиме отладки или на локальном компьютере.
        /// </summary>
        /// <param name="message">Сообщение для отображения.</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Создает новое окно сообщения об ошибке, содержащее исключение.
        /// Отображает текущую трассировку стека, если приложение работает в режиме отладки или на локальном компьютере.
        /// 
        /// Исключение (exception) преобразуется в сообщение с помощью метода <see cref="ConvertExceptionToMessage"/>.
        /// </summary>
        /// <param name="exception">Исключение (exception) для отображения.</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Создает новое окно сообщения об ошибке, содержащее исключение.
        /// Исключение (exception) преобразуется в сообщение с помощью метода <see cref="ConvertExceptionToMessage"/>.
        /// </summary>    
        /// <param name="exception">Исключение (exception) для отображения.</param>
        /// <param name="policy">Если нужно отобразить трассировку стека, см. раздел <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // Учитывать вложенные исключения (exceptions).
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// Создает новое окно сообщения об ошибке, содержащее указанное сообщение.
        /// </summary>   
        /// <param name="message">Сообщение для отображения.</param>
        /// <param name="policy">Если нужно отобразить трассировку стека, см. раздел <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region Фабричные методы
        /// <summary>
        /// Все остальные фабричные методы приводят к вызову этого.
        /// </summary>
        /// <param name="message">Сообщение для отображения</param>
        /// <param name="stackTrace">Связанная трассировка стека</param>
        /// <param name="policy">Случаи, в которых трассировка стека должна быть добавлена к сообщению</param>
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

        #region Помощники фабрики
        /// <summary>
        /// Возвращает значение, определяющее, что приложение запущено в режиме отладки или на локальном компьютере (localhost).
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
        /// Создает на основе исключения (Exception) понятное для пользователя сообщение. 
        /// В настоящее время этот метод просто возвращает значение свойства Exception.Message. 
        /// Этот метод можно изменить так, чтобы некоторые из классов исключений (Exception) трактовались по-другому.
        /// </summary>
        /// <param name="e">Исключение (exception) для преобразования.</param>
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