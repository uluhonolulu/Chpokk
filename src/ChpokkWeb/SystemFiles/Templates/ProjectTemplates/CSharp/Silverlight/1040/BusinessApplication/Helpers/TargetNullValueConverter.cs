namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Oggetto IValueConverter bidirezionale che consente di associare una proprietà di un oggetto associabile che può essere un valore di stringa vuoto a una proprietà di dipendenza che in tal caso deve essere impostata su null.
    /// </summary>
    public class TargetNullValueConverter : IValueConverter
    {
        /// <summary>
        /// Converte <c>null</c> o strings vuote in <c>null</c>.
        /// </summary>
        /// <param name="value">Valore da convertire.</param>
        /// <param name="targetType">Tipo previsto del risultato (ignorato).</param>
        /// <param name="parameter">Parametro facoltativo (ignorato).</param>
        /// <param name="culture">Impostazioni cultura per la conversione (ignorate).</param>
        /// <returns>Se <paramref name="value"/> è <c>null</c> o vuoto, questo metodo restituisce <c>null</c>; in caso contrario, restituisce <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;

            return string.IsNullOrEmpty(strValue) ? null : value;
        }

        /// <summary>
        /// Riconverte <c>null</c> in <see cref="String.Empty"/>.
        /// </summary>
        /// <param name="value">Valore da convertire.</param>
        /// <param name="targetType">Tipo previsto del risultato (ignorato).</param>
        /// <param name="parameter">Parametro facoltativo (ignorato).</param>
        /// <param name="culture">Impostazioni cultura per la conversione (ignorate).</param>
        /// <returns>Se <paramref name="value"/> è <c>null</c>, restituisce <see cref="String.Empty"/>; in caso contrario, <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ?? string.Empty;
        }
    }
}
