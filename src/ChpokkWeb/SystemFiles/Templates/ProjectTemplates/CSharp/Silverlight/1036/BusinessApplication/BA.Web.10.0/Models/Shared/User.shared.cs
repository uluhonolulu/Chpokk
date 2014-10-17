namespace $safeprojectname$
{
    /// <summary>
    /// Classe partielle étendant le type User qui ajoute des propriétés et des méthodes partagées.
    /// Ces propriétés et méthodes seront disponibles sur l'application serveur et sur l'application cliente.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Retourne le nom complet de l'utilisateur, qui est par défaut son FriendlyName.
        /// Si FriendlyName n'est pas défini, le nom d'utilisateur est retourné.
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
