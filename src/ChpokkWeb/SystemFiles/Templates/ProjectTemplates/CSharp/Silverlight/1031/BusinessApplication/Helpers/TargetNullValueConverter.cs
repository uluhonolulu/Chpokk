namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Zweiwege-IValueConverter, durch den eine Eigenschaft an ein bindbares Objekt gebunden werden kann, das aus einem leeren string-Wert für eine Abhängigkeitseigenschaft bestehen kann, die in diesem Fall auf null festgelegt werden sollte.
    /// </summary>
    public class TargetNullValueConverter : IValueConverter
    {
        /// <summary>
        /// Konvertiert <c>null</c> oder leere strings zu <c>null</c>.
        /// </summary>
        /// <param name="value">Der zu konvertierende Wert.</param>
        /// <param name="targetType">Der erwartete "type" des Ergebnisses (wird ignoriert).</param>
        /// <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
        /// <param name="culture">Die Kultur für die Konversion (wird ignoriert).</param>
        /// <returns>Wenn <paramref name="value"/> <c>null</c> oder leer ist, gibt diese Methode <c>null</c> zurück. Andernfalls wird der <paramref name="value"/> zurückgegeben.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;

            return string.IsNullOrEmpty(strValue) ? null : value;
        }

        /// <summary>
        /// Konvertiert <c>null</c> in <see cref="String.Empty"/> zurück.
        /// </summary>
        /// <param name="value">Der zu konvertierende Wert.</param>
        /// <param name="targetType">Der erwartete "type" des Ergebnisses (wird ignoriert).</param>
        /// <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
        /// <param name="culture">Die Kultur für die Konversion (wird ignoriert).</param>
        /// <returns>Falls <paramref name="value"/> <c>null</c> ist, wird <see cref="String.Empty"/> zurückgegeben, andernfalls <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ?? string.Empty;
        }
    }
}
