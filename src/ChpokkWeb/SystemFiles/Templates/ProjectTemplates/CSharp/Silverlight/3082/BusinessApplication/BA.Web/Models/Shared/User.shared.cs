namespace $safeprojectname$
{
    /// <summary>
    /// Clase parcial que extiende el tipo User que agrega propiedades y métodos compartidos.
    /// Estas propiedades y métodos estarán disponibles en la aplicación de servidor y en la aplicación cliente.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Devuelve el nombre para mostrar del usuario, que de forma predeterminada es su FriendlyName.
        /// Si no se ha establecido FriendlyName, se devuelve el nombre de usuario.
        /// </summary>
        public string DisplayName
        {
            get
            {
                if (!string.IsNullOrEmpty(this.FriendlyName))
                {
                    return this.FriendlyName;
                }
                else
                {
                    return this.Name;
                }
            }
        }
    }
}
