namespace $safeprojectname$
{
    /// <summary>
    /// Classe parziale che estende il tipo User per aggiungere proprietà e metodi condivisi.
    /// Tali proprietà e metodi sono disponibili sia per l'applicazione server che per l'applicazione client.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Restituisce il nome utente visualizzato, che per impostazione predefinita corrisponde all'oggetto FriendlyName.
        /// Se FriendlyName non è impostato, viene restituito l'oggetto User Name.
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
