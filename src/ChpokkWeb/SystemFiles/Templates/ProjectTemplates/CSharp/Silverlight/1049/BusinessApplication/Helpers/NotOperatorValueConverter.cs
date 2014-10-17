namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// Двусторонний <see cref="IValueConverter"/>, который позволяет привязывать отрицание логического (boolean) свойства к свойству зависимости (dependency property).
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует данное значение <paramref name="value"/> в его отрицание.
        /// </summary>
        /// <param name="value">Значение типа <c>bool</c> для преобразования.</param>
        /// <param name="targetType">Преобразуемый тип (type) (не используется).</param>
        /// <param name="parameter">Необязательный параметр (не используется).</param>
        /// <param name="culture">Культура преобразования (не используется).</param>
        /// <returns>Отрицание входного параметра <paramref name="value"/>.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// Отрицание <see cref="Convert"/>.
        /// </summary>
        /// <param name="value">Значение для обратного преобразования.</param>
        /// <param name="targetType">Преобразуемый тип (type) (не используется).</param>
        /// <param name="parameter">Необязательный параметр (не используется).</param>
        /// <param name="culture">Культура преобразования (не используется).</param>
        /// <returns>Отрицание входного параметра <paramref name="value"/>.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
