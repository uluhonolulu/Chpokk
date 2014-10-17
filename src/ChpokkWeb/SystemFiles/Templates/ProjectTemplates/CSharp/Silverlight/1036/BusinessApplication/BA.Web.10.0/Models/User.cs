namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// Classe contenant des informations sur l'utilisateur authentifié.
    /// </summary>
    public partial class User : UserBase
    {
        //// REMARQUE : des propriétés de profil peuvent être ajoutées pour être utilisées dans l'application Silverlight.
        //// Pour activer les profils, modifiez la section appropriée du fichier web.config.
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// Obtient et définit le nom convivial de l'utilisateur.
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
