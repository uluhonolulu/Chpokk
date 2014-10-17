Imports System
Imports System.Windows.Data

''' <summary>
''' 雙向 <see cref="IValueConverter"/>，可以讓您將 boolean 屬性的反向繫結到相依性屬性。
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' 將指定的 <paramref name="value"/> 轉換成它本身的反轉。
    ''' </summary>
    ''' <param name="value">要轉換的 <c>Boolean</c> 值。</param>
    ''' <param name="targetType">要轉換的目標 type (忽略)。</param>
    ''' <param name="parameter">選擇性參數 (忽略)。</param>
    ''' <param name="culture">此轉換的文化特性 (忽略)。</param>
    ''' <returns>輸入 <paramref name="value"/> 的反向。</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' <see cref="Convert"/> 的反向。
    ''' </summary>
    ''' <param name="value">要轉換回的值。</param>
    ''' <param name="targetType">要轉換的目標 type (忽略)。</param>
    ''' <param name="parameter">選擇性參數 (忽略)。</param>
    ''' <param name="culture">此轉換的文化特性 (忽略)。</param>
    ''' <returns>輸入 <paramref name="value"/> 的反向。</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class