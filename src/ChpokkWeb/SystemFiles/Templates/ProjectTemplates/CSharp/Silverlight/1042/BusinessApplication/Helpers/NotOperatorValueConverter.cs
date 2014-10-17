namespace $safeprojectname$
{
    using System;
    using System.Windows.Data;

    /// <summary>
    /// boolean 속성의 역을 종속성 속성에 바인딩할 수 있게 해주는 양방향 <see cref="IValueConverter"/>입니다.
    /// </summary>
    public class NotOperatorValueConverter : IValueConverter
    {
        /// <summary>
        /// 지정된 <paramref name="value"/>를 역으로 변환합니다.
        /// </summary>
        /// <param name="value">변환할 <c>bool</c> 값입니다.</param>
        /// <param name="targetType">변환할 type입니다(무시됨).</param>
        /// <param name="parameter">선택적 매개 변수입니다(무시됨).</param>
        /// <param name="culture">변환의 문화권입니다(무시됨).</param>
        /// <returns>입력 <paramref name="value"/>의 역입니다.</returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }

        /// <summary>
        /// <see cref="Convert"/>의 역입니다.
        /// </summary>
        /// <param name="value">다시 변환할 값입니다.</param>
        /// <param name="targetType">변환할 type입니다(무시됨).</param>
        /// <param name="parameter">선택적 매개 변수입니다(무시됨).</param>
        /// <param name="culture">변환의 문화권입니다(무시됨).</param>
        /// <returns>입력 <paramref name="value"/>의 역입니다.</returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
