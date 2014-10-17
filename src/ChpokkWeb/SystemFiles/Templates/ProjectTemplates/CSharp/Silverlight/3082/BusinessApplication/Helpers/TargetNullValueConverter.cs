namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// IValueConverter bidireccional que permite enlazar una propiedad de un objeto enlazable que puede ser un valor de string vacía a una propiedad de dependencia que en ese caso debería establecerse en null.
    /// </summary>
    public class TargetNullValueConverter : IValueConverter
    {
        /// <summary>
        /// Convierte strings <c>null</c> o vacías en <c>null</c>.
        /// </summary>
        /// <param name="value">Valor que se va a convertir.</param>
        /// <param name="targetType">Es el type esperado del resultado (omitido).</param>
        /// <param name="parameter">Parámetro opcional (omitido).</param>
        /// <param name="culture">Referencia cultural para la conversión (omitida).</param>
        /// <returns>Si el <paramref name="value"/>es <c>null</c> o está vacío, este método devuelve <c>null</c>, de lo contrario, devuelve el <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;

            return string.IsNullOrEmpty(strValue) ? null : value;
        }

        /// <summary>
        /// Revierte <c>null</c> a <see cref="String.Empty"/>.
        /// </summary>
        /// <param name="value">Valor que se va a convertir.</param>
        /// <param name="targetType">Type esperado del resultado (omitido).</param>
        /// <param name="parameter">Parámetro opcional (omitido).</param>
        /// <param name="culture">Referencia cultural para la conversión (omitida).</param>
        /// <returns>Si <paramref name="value"/> es <c>null</c>, devuelve <see cref="String.Empty"/>, de lo contrario, <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ?? string.Empty;
        }
    }
}
