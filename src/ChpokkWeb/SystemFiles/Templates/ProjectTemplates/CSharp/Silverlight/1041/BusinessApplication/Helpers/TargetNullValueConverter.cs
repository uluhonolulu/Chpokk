namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// 空の string 値を指定できるバインド可能なオブジェクトのプロパティを依存関係プロパティ (その場合、null に設定する必要があります) にバインドできる、双方向の IValueConverter です。
    /// </summary>
    public class TargetNullValueConverter : IValueConverter
    {
        /// <summary>
        /// <c>null</c> または空の strings を <c>null</c> に変換します。
        /// </summary>
        /// <param name="value">変換する値です。</param>
        /// <param name="targetType">結果に必要な型です (無視)。</param>
        /// <param name="parameter">省略可能なパラメーターです (無視)。</param>
        /// <param name="culture">変換のカルチャです (無視)。</param>
        /// <returns><paramref name="value"/> が <c>null</c> または空の場合、このメソッドは <c>null</c> を返し、それ以外の場合は <paramref name="value"/> を返します。</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strValue = value as string;

            return string.IsNullOrEmpty(strValue) ? null : value;
        }

        /// <summary>
        /// <c>null</c> を変換して <see cref="String.Empty"/> に戻します。
        /// </summary>
        /// <param name="value">変換する値です。</param>
        /// <param name="targetType">結果に必要な型です (無視)。</param>
        /// <param name="parameter">省略可能なパラメーターです (無視)。</param>
        /// <param name="culture">変換のカルチャです (無視)。</param>
        /// <returns><paramref name="value"/> が <c>null</c> の場合は <see cref="String.Empty"/> を返し、それ以外の場合は <paramref name="value"/> を返します。</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value ?? string.Empty;
        }
    }
}
