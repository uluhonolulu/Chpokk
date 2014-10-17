namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// Klasse, die Informationen über den authentifizierten Benutzer enthält.
    /// </summary>
    public partial class User : UserBase
    {
        //// HINWEIS: Profileigenschaften können für die Verwendung in der Silverlight-Anwendung hinzugefügt werden.
        //// Um Profile zu aktivieren, bearbeiten Sie den entsprechenden Abschnitt der Datei "web.config".
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// Ruft den Anzeigenamen des Benutzers ab und legt ihn fest.
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
