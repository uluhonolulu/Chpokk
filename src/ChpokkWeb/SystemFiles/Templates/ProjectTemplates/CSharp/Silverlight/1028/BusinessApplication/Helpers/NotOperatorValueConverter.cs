namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// 雙向 <see cref="IValueConverter"/>，可以讓您將 boolean 屬性的反向繫結到相依性屬性。
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// 將指定的 <paramref name="value"/> 轉換成它本身的反轉。
        /// </summary>
        /// <param name="value">要轉換的 <c>bool</c> 值。</param>
        /// <param name="targetType">要轉換的目標 type (忽略)。</param>
        /// <param name="parameter">選擇性參數 (忽略)。</param>
        /// <param name="culture">此轉換的文化特性 (忽略)。</param>
        /// <returns>輸入 <paramref name="value"/> 的反向。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// <see cref="Convert"/> 的反向。
        /// </summary>
        /// <param name="value">要轉換回的值。</param>
        /// <param name="targetType">要轉換的目標 type (忽略)。</param>
        /// <param name="parameter">選擇性參數 (忽略)。</param>
        /// <param name="culture">此轉換的文化特性 (忽略)。</param>
        /// <returns>輸入 <paramref name="value"/> 的反向。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
