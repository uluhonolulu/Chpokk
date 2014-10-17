namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// 双向 <see cref="IValueConverter"/>，使您可以将 boolean 属性的反向值绑定到依赖属性。
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// 将给定 <paramref name="value"/> 转换为其反向值。
        /// </summary>
        /// <param name="value">要转换的<c>bool</c>值。</param>
        /// <param name="targetType">要转换到的 type (已忽略)。</param>
        /// <param name="parameter">可选参数(已忽略)。</param>
        /// <param name="culture">转换的区域性(已忽略)。</param>
        /// <returns>输入 <paramref name="value"/> 的反向值。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// <see cref="Convert"/> 的反向值。
        /// </summary>
        /// <param name="value">要转换回的值。</param>
        /// <param name="targetType">要转换到的 type (已忽略)。</param>
        /// <param name="parameter">可选参数(已忽略)。</param>
        /// <param name="culture">转换的区域性(已忽略)。</param>
        /// <returns>输入 <paramref name="value"/> 的反向值。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
