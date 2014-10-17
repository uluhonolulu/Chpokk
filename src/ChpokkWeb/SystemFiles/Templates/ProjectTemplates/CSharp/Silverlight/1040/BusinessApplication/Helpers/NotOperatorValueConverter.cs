namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Oggetto <see cref="IValueConverter"/> bidirezionale che consente di associare l'inverso di una proprietà booleana a una proprietà di dipendenza.
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// Converte l'oggetto <paramref name="value"/> specificato nel relativo inverso.
        /// </summary>
        /// <param name="value">Valore <c>bool</c> da convertire.</param>
        /// <param name="targetType">Tipo in cui convertire (ignorato).</param>
        /// <param name="parameter">Parametro facoltativo (ignorato).</param>
        /// <param name="culture">Impostazioni cultura della conversione (ignorate).</param>
        /// <returns>Inverso dell'oggetto <paramref name="value"/> di input.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// Inverso dell'oggetto <see cref="Convert"/>.
        /// </summary>
        /// <param name="value">Valore da riconvertire.</param>
        /// <param name="targetType">Tipo in cui convertire (ignorato).</param>
        /// <param name="parameter">Parametro facoltativo (ignorato).</param>
        /// <param name="culture">Impostazioni cultura della conversione (ignorate).</param>
        /// <returns>Inverso dell'oggetto <paramref name="value"/> di input.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
