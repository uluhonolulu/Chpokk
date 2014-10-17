Imports System
Imports System.Windows.Data

''' <summary>
''' boolean 속성의 역을 종속성 속성에 바인딩할 수 있게 해주는 양방향 <see cref="IValueConverter"/>입니다.
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' 지정된 <paramref name="value"/>를 역으로 변환합니다.
    ''' </summary>
    ''' <param name="value">변환할 <c>Boolean</c> 값입니다.</param>
    ''' <param name="targetType">변환할 type입니다(무시됨).</param>
    ''' <param name="parameter">선택적 매개 변수입니다(무시됨).</param>
    ''' <param name="culture">변환의 문화권입니다(무시됨).</param>
    ''' <returns>입력 <paramref name="value"/>의 역입니다.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' <see cref="Convert"/>의 역입니다.
    ''' </summary>
    ''' <param name="value">다시 변환할 값입니다.</param>
    ''' <param name="targetType">변환할 type입니다(무시됨).</param>
    ''' <param name="parameter">선택적 매개 변수입니다(무시됨).</param>
    ''' <param name="culture">변환의 문화권입니다(무시됨).</param>
    ''' <returns>입력 <paramref name="value"/>의 역입니다.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class