namespace $safeprojectname$
{
    using System;
    using System.Diagnostics;
    using System.Windows;
    using System.Windows.Controls;
    using System.Net;

    /// <summary>
    /// Contrôle à quel moment la trace de la pile doit s'afficher dans l'ErrorWindow.
    /// La valeur par défaut est <see cref="OnlyWhenDebuggingOrRunningLocally"/>
    /// </summary>
    public enum StackTracePolicy
    {
        /// <summary>
        ///   La trace de la pile est affichée lors d'une exécution avec un débogueur attaché ou lorsque l'application est exécutée sur l'ordinateur local.
        ///   Utilisez-la pour obtenir des informations de débogage supplémentaires que vous ne voulez pas afficher aux utilisateurs finals.
        /// </summary>
        OnlyWhenDebuggingOrRunningLocally,

        /// <summary>
        /// Toujours afficher la trace de la pile, même en cas de débogage
        /// </summary>
        Always,

        /// <summary>
        /// Ne jamais afficher la trace de la pile, même lors du débogage
        /// </summary>
        Never
    }

    /// <summary>
    /// Classe <see cref="ChildWindow"/> qui affiche les erreurs à l'utilisateur.
    /// </summary>
    public partial class ErrorWindow : ChildWindow
    {
        /// <summary>
        /// Crée une nouvelle instance <see cref="ErrorWindow"/>.
        /// </summary>
        /// <param name="message">Message d'erreur à afficher.</param>
        /// <param name="errorDetails">Informations supplémentaires sur l'erreur.</param>
        protected ErrorWindow(string message, string errorDetails)
        {
            InitializeComponent();
            IntroductoryText.Text = message;
            ErrorTextBox.Text = errorDetails;
        }

        #region Méthodes rapides de fabrique
        /// <summary>
        /// Crée une nouvelle fenêtre d'erreurs avec un message d'erreur.
        /// La trace de la pile actuelle s'affiche si l'application est en cours d'exécution lors du débogage ou sur un ordinateur local.
        /// </summary>
        /// <param name="message">Message à afficher.</param>
        public static void CreateNew(string message)
        {
            CreateNew(message, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Crée une nouvelle fenêtre d'erreurs avec une exception.
        /// La trace de la pile actuelle s'affiche si l'application est en cours d'exécution lors du débogage ou sur un ordinateur local.
        /// 
        /// L'exception est convertie en message à l'aide de <see cref="ConvertExceptionToMessage"/>.
        /// </summary>
        /// <param name="exception">Exception à afficher.</param>
        public static void CreateNew(Exception exception)
        {
            CreateNew(exception, StackTracePolicy.OnlyWhenDebuggingOrRunningLocally);
        }

        /// <summary>
        /// Crée une nouvelle fenêtre d'erreurs avec une exception.
        /// L'exception est convertie en message à l'aide de <see cref="ConvertExceptionToMessage"/>.
        /// </summary>    
        /// <param name="exception">Exception à afficher.</param>
        /// <param name="policy">Au moment d'afficher la trace de la pile, consulter <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(Exception exception, StackTracePolicy policy)
        {
           if (exception == null)
           {
               throw new ArgumentNullException("exception");
           }

           string fullStackTrace = exception.StackTrace;

            // Compte pour les exceptions imbriquées
            Exception innerException = exception.InnerException;
            while (innerException != null)
            {
                fullStackTrace += "\n" + string.Format(System.Globalization.CultureInfo.CurrentUICulture, ErrorResources.ErrorWindowInnerException, innerException.Message) + "\n\n" + innerException.StackTrace;
                innerException = innerException.InnerException;
            }

            CreateNew(ConvertExceptionToMessage(exception), fullStackTrace, policy);
        }

        /// <summary>
        /// Crée une nouvelle fenêtre d'erreurs avec un message d'erreur.
        /// </summary>   
        /// <param name="message">Message à afficher.</param>
        /// <param name="policy">Au moment d'afficher la trace de la pile, consulter <see cref="StackTracePolicy"/>.</param>
        public static void CreateNew(string message, StackTracePolicy policy)
        {
            CreateNew(message, new StackTrace().ToString(), policy);
        }
        #endregion

        #region Méthodes de fabrique
        /// <summary>
        /// Toutes les autres méthodes de fabrique entraîneront un appel à celle-ci.
        /// </summary>
        /// <param name="message">Message à afficher</param>
        /// <param name="stackTrace">Trace de la pile associée</param>
        /// <param name="policy">Situations dans lesquelles la trace de la pile doit être ajoutée au message</param>
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

        #region Programmes d'assistance de fabrique
        /// <summary>
        /// Retourne une valeur indiquant si l'exécution s'effectue avec un débogueur attaché ou avec le serveur hébergé sur localhost.
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
        /// Crée un message convivial pour l'utilisateur avec une Exception donnée. 
        /// Actuellement, cette méthode retourne la valeur Exception.Message. 
        /// Vous pouvez modifier cette méthode pour traiter certaines classes Exception différemment.
        /// </summary>
        /// <param name="e">Exception à convertir.</param>
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