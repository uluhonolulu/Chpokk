namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// Steuert, wann eine Stapelüberwachung in ErrorWindow angezeigt werden soll.
    /// Der Standardwert lautet <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   "Stapelüberwachung" wird angezeigt, wenn die Anwendung mit einem Debugger verbunden ist oder auf dem lokalen Computer ausgeführt wird.
        ///   Verwenden Sie dies zum Anzeigen zusätzlicher Debuginformationen, die dem Endbenutzer nicht angezeigt werden sollen.
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// Die Stapelüberwachung immer anzeigen, auch beim Debugging
        /// </summary>
        Always,

        /// <summary>
        /// Die Stapelüberwachung niemals anzeigen, auch nicht beim Debugging
        /// </summary>
        Never
    }

    /// <summary>
    /// <see cref="ChildWindow"/> Klasse, die dem Benutzer Fehler anzeigt.
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// Erstellt eine neue Instanz von <see cref="ErrorWindow"/>.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, die angezeigt wird.</param>
        /// <param name="errorDetails">Zusätzliche Fehlerinformationen.</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region Factory Shortcut-Methoden
        /// <summary>
        /// Zeigt ein Fehlerfenster an, falls eine Fehlermeldung vorhanden ist.
        /// Aktuelle Stapelüberwachung wird angezeigt, wenn die Anwendung im Debugmodus oder auf dem lokalen Computer ausgeführt wird.
        /// </summary>
        /// <param name="message">Die Meldung, die angezeigt wird.</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Erstellt ein neues Fehlerfenster, falls eine Ausnahme vorhanden ist.
        /// Aktuelle Stapelüberwachung wird angezeigt, wenn die Anwendung im Debugmodus oder auf dem lokalen Computer ausgeführt wird.
        /// 
        /// Die Ausnahme wird mithilfe von <see cref="ConvertExceptionToMessage"/> in eine Nachricht umgewandelt.
        /// </summary>
        /// <param name="exception">Die Ausnahme, die angezeigt wird.</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Erstellt ein neues Fehlerfenster, falls eine Ausnahme vorhanden ist.
        /// Die Ausnahme wird mithilfe von <see cref="ConvertExceptionToMessage"/> in eine Nachricht umgewandelt.
        /// </summary>    
        /// <param name="exception">Die Ausnahme, die angezeigt wird.</param>
        /// <param name="policy">Siehe <see cref="StackTracePolicy"/>, wann die Stapelüberwachung angezeigt wird.</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // Konto für geschachtelte Ausnahmen
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// Zeigt ein Fehlerfenster an, falls eine Fehlermeldung vorhanden ist.
        /// </summary>   
        /// <param name="message">Die Meldung, die angezeigt wird.</param>
        /// <param name="policy">Siehe <see cref="StackTracePolicy"/>, wann die Stapelüberwachung angezeigt wird.</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region Factory-Methoden
        /// <summary>
        /// Alle anderen Factory-Methoden haben einen Aufruf dieser Methode zur Folge.
        /// </summary>
        /// <param name="message">Die Fehlermeldung, die angezeigt wird</param>
        /// <param name="stackTrace">Die zugeordnete Stapelüberwachung</param>
        /// <param name="policy">Die Situation, in der die Stapelüberwachung an die Nachricht angehängt werden soll.</param>
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

        #region Factory-Hilfsprogramme
        /// <summary>
        /// Gibt zurück, ob eine Verbindung zu einem Debugger besteht oder der Server auf localhost gehostet ist.
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
        /// Erstellt im Falle einer Ausnahme eine benutzerfreundliche Nachricht. 
        /// Momentan gibt diese Methode den Wert von Exception.Message zurück. 
        /// Diese Methode kann geändert werden, um bestimmte "Exception"-Klassen unterschiedlich zu behandeln.
        /// </summary>
        /// <param name="e">Die zu konvertierende Ausnahme.</param>
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