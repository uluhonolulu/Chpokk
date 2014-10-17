namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// <see cref="IValueConverter"/> bidirectionnel qui vous permet de lier l'inverse d'une propriété boolean à une propriété de dépendance.
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// Convertit le <paramref name="value"/> donné pour qu'il soit son inverse.
        /// </summary>
        /// <param name="value">Valeur <c>bool</c> à convertir.</param>
        /// <param name="targetType">Type dans lequel effectuer la conversion (ignoré).</param>
        /// <param name="parameter">Paramètre optionnel (ignoré).</param>
        /// <param name="culture">Culture de la conversion (ignorée).</param>
        /// <returns>Inverse de l'entrée <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// Inverse de <see cref="Convert"/>.
        /// </summary>
        /// <param name="value">Valeur vers laquelle effectuer la reconversion.</param>
        /// <param name="targetType">Type dans lequel effectuer la conversion (ignoré).</param>
        /// <param name="parameter">Paramètre optionnel (ignoré).</param>
        /// <param name="culture">Culture de la conversion (ignorée).</param>
        /// <returns>Inverse de l'entrée <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
