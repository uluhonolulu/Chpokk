namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// boolean プロパティの逆数を依存関係プロパティにバインドできる双方向の <see cref="IValueConverter"/> です。
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// 指定された <paramref name="value"/> を逆数に変換します。
        /// </summary>
        /// <param name="value">変換する <c>bool</c> 値です。</param>
        /// <param name="targetType">変換先の型です (無視)。</param>
        /// <param name="parameter">省略可能なパラメーターです (無視)。</param>
        /// <param name="culture">変換のカルチャです (無視)。</param>
        /// <returns>入力 <paramref name="value"/> の逆数です。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// <see cref="Convert"/> の逆数です。
        /// </summary>
        /// <param name="value">変換によって元に戻された後の値です。</param>
        /// <param name="targetType">変換先の型です (無視)。</param>
        /// <param name="parameter">省略可能なパラメーターです (無視)。</param>
        /// <param name="culture">変換のカルチャです (無視)。</param>
        /// <returns>入力 <paramref name="value"/> の逆数です。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
