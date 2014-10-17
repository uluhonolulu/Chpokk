namespace $safeprojectname$
{
    /// <summary>
    /// Teilklasse, die den User-Typ erweitert, durch den freigegebene Eigenschaften und Methoden hinzugefügt werden.
    /// Diese Eigenschaften und Methoden sind sowohl für die Server- als auch für die Clientanwendung verfügbar.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Gibt den Benutzeranzeigenamen zurück, welcher standardmäßig dem FriendlyName entspricht.
        /// Wenn FriendlyName nicht festgelegt ist, wird der Benutzername zurückgegeben.
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
