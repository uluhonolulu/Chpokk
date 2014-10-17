namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Zweiwege-<see cref="IValueConverter"/>, durch den die Inversion einer "boolean"-Eigenschaft an eine Abhängigkeitseigenschaft gebunden werden kann.
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// Konvertiert den angegebenen <paramref name="value"/> in sein Gegenteil (Inversion).
        /// </summary>
        /// <param name="value">Der <c>bool</c> Wert, der konvertiert werden soll.</param>
        /// <param name="targetType">Der "type", in den konvertiert werden soll (wird ignoriert).</param>
        /// <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
        /// <param name="culture">Die Kultur der Konversion (wird ignoriert).</param>
        /// <returns>Die Inversion des Eingabe-<paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// Die Inversion der <see cref="Convert"/>.
        /// </summary>
        /// <param name="value">Der zurückzukonvertierende Wert.</param>
        /// <param name="targetType">Der "type", in den konvertiert werden soll (wird ignoriert).</param>
        /// <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
        /// <param name="culture">Die Kultur der Konversion (wird ignoriert).</param>
        /// <returns>Die Inversion des Eingabe-<paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
