namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Двусторонний IValueConverter, который позволяет привязывать свойство привязываемого объекта, которое может быть пустой строкой (string), к свойству зависимости (dependency property), которое должно быть в данном случае установлено в значение null.
    /// </summary>
    public class TargetNullValueConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует значения <c>null</c> и пустые строки (strings) в значения <c>null</c>.
        /// </summary>
        /// <param name="value">Значение для преобразования.</param>
        /// <param name="targetType">Ожидаемый тип (type) преобразования (не используется).</param>
        /// <param name="parameter">Необязательный параметр (не используется).</param>
        /// <param name="culture">Культура преобразования (не используется).</param>
        /// <returns>Если <paramref name="value"/> имеет значение <c>null</c> или пуст, этот метод возвращает значение <c>null</c>. В противном случае возвращается значение <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;

            return string.IsNullOrEmpty(strValue) ? null : value;
        }

        /// <summary>
        /// Преобразует значение <c>null</c> обратно в <see cref="String.Empty"/>.
        /// </summary>
        /// <param name="value">Значение для преобразования.</param>
        /// <param name="targetType">Ожидаемый тип преобразования (не используется).</param>
        /// <param name="parameter">Необязательный параметр (не используется).</param>
        /// <param name="culture">Культура преобразования (не используется).</param>
        /// <returns>Если <paramref name="value"/> имеет значение <c>null</c>, возвращается <see cref="String.Empty"/>. В противном случае возвращается <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ?? string.Empty;
        }
    }
}
