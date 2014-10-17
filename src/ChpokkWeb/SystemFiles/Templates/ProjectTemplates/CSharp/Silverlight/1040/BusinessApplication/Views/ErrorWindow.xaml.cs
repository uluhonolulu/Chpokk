namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// Controlla quando visualizzare una traccia dello stack in ErrorWindow.
    /// Impostazione predefinita: <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   La traccia dello stack viene visualizzata se l'applicazione viene eseguita con un debugger associato o nel computer locale.
        ///   Da utilizzare per ottenere ulteriori informazioni sul debug che non si desidera vengano visualizzate dagli utenti finali.
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// Mostrare sempre la traccia dello stack, anche durante il debug
        /// </summary>
        Always,

        /// <summary>
        /// Non mostrare la traccia dello stack, neanche durante il debug
        /// </summary>
        Never
    }

    /// <summary>
    /// Classe <see cref="ChildWindow"/> che visualizza gli errori all'utente.
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// Crea una nuova istanza di <see cref="ErrorWindow"/>.
        /// </summary>
        /// <param name="message">Messaggio di errore da visualizzare.</param>
        /// <param name="errorDetails">Informazioni aggiuntive sull'errore.</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region Metodi rapidi factory
        /// <summary>
        /// Crea una nuova finestra di errore in base a un messaggio di errore.
        /// La traccia dello stack corrente verrà visualizzata se l'applicazione viene eseguita durante il debug o nel computer locale.
        /// </summary>
        /// <param name="message">Messaggio da visualizzare.</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Crea una nuova finestra di errore in base a un'eccezione.
        /// La traccia dello stack corrente verrà visualizzata se l'applicazione viene eseguita durante il debug o nel computer locale.
        /// 
        /// L'eccezione viene convertita in un messaggio mediante <see cref="ConvertExceptionToMessage"/>.
        /// </summary>
        /// <param name="exception">L'eccezione da visualizzare.</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Crea una nuova finestra di errore in base a un'eccezione.
        /// L'eccezione viene convertita in un messaggio mediante <see cref="ConvertExceptionToMessage"/>.
        /// </summary>    
        /// <param name="exception">L'eccezione da visualizzare.</param>
        /// <param name="policy">Per stabilire quando visualizzare la traccia dello stack, vedere <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // Account per eccezioni annidate
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// Crea una nuova finestra di errore in base a un messaggio di errore.
        /// </summary>   
        /// <param name="message">Messaggio da visualizzare.</param>
        /// <param name="policy">Per stabilire quando visualizzare la traccia dello stack, vedere <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region Metodi factory
        /// <summary>
        /// Tutti gli altri metodi factory determineranno una chiamata a questo metodo.
        /// </summary>
        /// <param name="message">Messaggio da visualizzare</param>
        /// <param name="stackTrace">Traccia dello stack associata</param>
        /// <param name="policy">Situazioni in cui aggiungere la traccia dello stack al messaggio</param>
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

        #region Supporti factory
        /// <summary>
        /// Restituisce un valore che indica se eseguire l'applicazione con un debugger associato o con il server ospitato su localhost.
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
        /// Crea un messaggio descrittivo in base a un'eccezione. 
        /// Attualmente questo metodo restituisce il valore Exception.Message. 
        /// È possibile modificare questo metodo in modo da considerare alcune classi Exception in modo diverso.
        /// </summary>
        /// <param name="e">L'eccezione da convertire.</param>
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