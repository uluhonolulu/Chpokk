namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// IValueConverter bidirectionnel qui vous permet de lier une propriété sur un objet pouvant être lié qui peut être une valeur string vide à une propriété de dépendance à laquelle la valeur null doit être affectée dans ce cas.
    /// </summary>
    public class TargetNullValueConverter : IValueConverter
    {
        /// <summary>
        /// Convertit les strings <c>null</c> ou vides en <c>null</c>.
        /// </summary>
        /// <param name="value">Valeur à convertir.</param>
        /// <param name="targetType">Type attendu du résultat (ignoré).</param>
        /// <param name="parameter">Paramètre optionnel (ignoré).</param>
        /// <param name="culture">Culture de la conversion (ignorée).</param>
        /// <returns>Si <paramref name="value"/> a la valeur <c>null</c> ou est vide, cette méthode retourne <c>null</c> ; sinon, elle retourne <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;

            return string.IsNullOrEmpty(strValue) ? null : value;
        }

        /// <summary>
        /// Reconvertit <c>null</c> en <see cref="String.Empty"/>.
        /// </summary>
        /// <param name="value">Valeur à convertir.</param>
        /// <param name="targetType">Type attendu du résultat (ignoré).</param>
        /// <param name="parameter">Paramètre optionnel (ignoré).</param>
        /// <param name="culture">Culture de la conversion (ignorée).</param>
        /// <returns>Si <paramref name="value"/> a la valeur <c>null</c>, <see cref="String.Empty"/> est retourné ; sinon, <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ?? string.Empty;
        }
    }
}
