namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// <see cref="IValueConverter"/> bidireccional que permite enlazar el inverso de una propiedad boolean a una dependency property.
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// Convierte el <paramref name="value"/> proporcionado en su inverso.
        /// </summary>
        /// <param name="value">Valor <c>bool</c> que se va a convertir.</param>
        /// <param name="targetType">Es el type al que se va a convertir (omitido).</param>
        /// <param name="parameter">Parámetro opcional (omitido).</param>
        /// <param name="culture">Referencia cultural de la conversión (omitida).</param>
        /// <returns>Inverso del <paramref name="value"/> de entrada.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// Inverso de <see cref="Convert"/>.
        /// </summary>
        /// <param name="value">Valor que se va a revertir.</param>
        /// <param name="targetType">Es el type al que se va a convertir (omitido).</param>
        /// <param name="parameter">Parámetro opcional (omitido).</param>
        /// <param name="culture">Referencia cultural de la conversión (omitida).</param>
        /// <returns>Inverso del <paramref name="value"/> de entrada.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
