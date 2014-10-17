Imports System
Imports System.Windows.Data

''' <summary>
''' 빈 string 값을 가질 수 있는 바인딩 가능한 개체에 대한 속성을 빈 문자열 값을 null로 설정해야 하는 종속성 속성에 바인딩할 수 있도록 해주는 양방향 IValueConverter입니다.
''' </summary>
Public Class TargetNullValueConverter
    Implements IValueConverter

    ''' <summary>
    ''' <c>null</c> 또는 빈 strings를 <c>null</c>로 변환합니다.
    ''' </summary>
    ''' <param name="value">변환할 값입니다.</param>
    ''' <param name="targetType">결과의 예상 형식입니다(무시됨).</param>
    ''' <param name="parameter">선택적 매개 변수입니다(무시됨).</param>
    ''' <param name="culture">변환의 문화권입니다(무시됨).</param>
    ''' <returns><paramref name="value"/>가 <c>null</c>이거나 비어 있으면 이 메서드는 <c>null</c>을 반환하고, 그렇지 않으면 <paramref name="value"/>를 반환합니다.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim strValue As String = Nothing

        If TypeOf value Is String Then
            strValue = DirectCast(value, String)
        End If

        Return If(String.IsNullOrEmpty(strValue), Nothing, value)
    End Function

    ''' <summary>
    ''' <c>null</c>을 <see cref="String.Empty"/>로 다시 변환합니다.
    ''' </summary>
    ''' <param name="value">변환할 값입니다.</param>
    ''' <param name="targetType">결과의 예상 형식입니다(무시됨).</param>
    ''' <param name="parameter">선택적 매개 변수입니다(무시됨).</param>
    ''' <param name="culture">변환의 문화권입니다(무시됨).</param>
    ''' <returns><paramref name="value"/>가 <c>null</c>이면 <see cref="String.Empty"/>를 반환하고, 그렇지 않으면 <paramref name="value"/>를 반환합니다.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value Is Nothing, String.Empty, value)
    End Function
End Class