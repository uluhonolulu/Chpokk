namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// Controla cuándo tiene que mostrarse un seguimiento de la pila en la ErrorWindow.
    /// Se establece de forma predeterminada en <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   El seguimiento de la pila se muestra al ejecutar con un depurador adjunto o al ejecutar una aplicación en la máquina local.
        ///   Utilice esto para obtener información adicional de depuración que no desea que vean los usuarios finales.
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// Mostrar siempre el seguimiento de la pila, aunque se esté depurando
        /// </summary>
        Always,

        /// <summary>
        /// No mostrar nunca el seguimiento de la pila, ni al depurar
        /// </summary>
        Never
    }

    /// <summary>
    /// Clase <see cref="ChildWindow"/> que muestra errores al usuario.
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// Crea una nueva instancia de <see cref="ErrorWindow"/>.
        /// </summary>
        /// <param name="message">Mensaje de error que se va a mostrar.</param>
        /// <param name="errorDetails">Información adicional sobre el error.</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region Métodos abreviados de fábrica
        /// <summary>
        /// Crea una nueva ventana de error en función de un mensaje de error.
        /// Si la aplicación se está ejecutando en modo depuración o en la máquina local, se mostrará el seguimiento de la pila actual.
        /// </summary>
        /// <param name="message">Mensaje que se va a mostrar.</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Crea una nueva ventana de error en función de una exception.
        /// Si la aplicación se está ejecutando en modo depuración o en la máquina local, se mostrará el seguimiento de la pila actual.
        /// 
        /// La exception se convierte en un mensaje mediante <see cref="ConvertExceptionToMessage"/>.
        /// </summary>
        /// <param name="exception">Es la exception que se va a mostrar.</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Crea una nueva ventana de error en función de una exception.
        /// La exception se convierte en un mensaje mediante <see cref="ConvertExceptionToMessage"/>.
        /// </summary>    
        /// <param name="exception">Es la exception que se va a mostrar.</param>
        /// <param name="policy">Si va a mostrar el seguimiento de la pila, consulte <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // Cuenta para exceptions anidadas
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// Crea una nueva ventana de error en función de un mensaje de error.
        /// </summary>   
        /// <param name="message">Mensaje que se va a mostrar.</param>
        /// <param name="policy">Si va a mostrar el seguimiento de la pila, consulte <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region Métodos de fábrica
        /// <summary>
        /// Todos los demás métodos de fábrica darán lugar a una llamada a éste.
        /// </summary>
        /// <param name="message">Mensaje que se va a mostrar</param>
        /// <param name="stackTrace">Seguimiento de la pila asociado</param>
        /// <param name="policy">Situaciones en que el seguimiento de la pila debería anexarse al mensaje</param>
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

        #region Aplicaciones auxiliares de fábrica
        /// <summary>
        /// Devuelve si se está ejecutando con un depurador adjunto o con el servidor hospedado en localhost.
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
        /// Crea un mensaje fácil de usar en función de una Exception. 
        /// En este momento este método devuelve el valor Exception.Message. 
        /// Se puede modificar este método para tratar ciertas clases Exception de forma distinta.
        /// </summary>
        /// <param name="e">Es la exception que se va a convertir.</param>
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