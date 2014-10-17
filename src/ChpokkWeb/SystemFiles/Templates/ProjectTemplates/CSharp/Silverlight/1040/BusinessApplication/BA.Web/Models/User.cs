namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// Classe contenente le informazioni sull'utente autenticato.
    /// </summary>
    public partial class User : UserBase
    {
        //// NOTA: è possibile aggiungere le proprietà del profilo da utilizzare nell'applicazione Silverlight.
        //// Per abilitare i profili, modificare la sezione appropriata del file web.config.
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// Ottiene e imposta il nome descrittivo dell'utente.
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
