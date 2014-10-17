Imports System
Imports System.Windows.Data

''' <summary>
''' 双向 <see cref="IValueConverter"/>，使您可以将 boolean 属性的反向值绑定到依赖属性。
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' 将给定 <paramref name="value"/> 转换为其反向值。
    ''' </summary>
    ''' <param name="value">要转换的 <c>Boolean</c> 值。</param>
    ''' <param name="targetType">要转换到的 type (已忽略)。</param>
    ''' <param name="parameter">可选参数(已忽略)。</param>
    ''' <param name="culture">转换的区域性(已忽略)。</param>
    ''' <returns>输入 <paramref name="value"/> 的反向值。</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' <see cref="Convert"/> 的反向值。
    ''' </summary>
    ''' <param name="value">要转换回的值。</param>
    ''' <param name="targetType">要转换到的 type (已忽略)。</param>
    ''' <param name="parameter">可选参数(已忽略)。</param>
    ''' <param name="culture">转换的区域性(已忽略)。</param>
    ''' <returns>输入 <paramref name="value"/> 的反向值。</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class