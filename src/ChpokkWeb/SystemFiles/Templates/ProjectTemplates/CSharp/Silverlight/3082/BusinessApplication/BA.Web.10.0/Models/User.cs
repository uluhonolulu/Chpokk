namespace $safeprojectname$
{
    using System.Runtime.Serialization;
    using System.ServiceModel.DomainServices.Server.ApplicationServices;

    /// <summary>
    /// Clase que contiene información sobre el usuario autenticado.
    /// </summary>
    public partial class User : UserBase
    {
        //// NOTA: se pueden agregar propiedades de perfil para utilizarlas en la aplicación de Silverlight.
        //// Para habilitar perfiles, edite la sección correspondiente del archivo web.config.
        ////
        //// public string MyProfileProperty { get; set; }

        /// <summary>
        /// Obtiene y establece el nombre descriptivo del usuario.
        /// </summary>
        public string FriendlyName { get; set; }
    }
}
