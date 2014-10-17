Imports System
Imports System.Windows.Data

''' <summary>
''' boolean プロパティの逆数を依存関係プロパティにバインドできる双方向の <see cref="IValueConverter"/> です。
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' 指定された <paramref name="value"/> を逆数に変換します。
    ''' </summary>
    ''' <param name="value">変換する <c>Boolean</c> 値です。</param>
    ''' <param name="targetType">変換先の型です (無視)。</param>
    ''' <param name="parameter">省略可能なパラメーターです (無視)。</param>
    ''' <param name="culture">変換のカルチャです (無視)。</param>
    ''' <returns>入力 <paramref name="value"/> の逆数です。</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' <see cref="Convert"/> の逆数です。
    ''' </summary>
    ''' <param name="value">変換によって元に戻された後の値です。</param>
    ''' <param name="targetType">変換先の型です (無視)。</param>
    ''' <param name="parameter">省略可能なパラメーターです (無視)。</param>
    ''' <param name="culture">変換のカルチャです (無視)。</param>
    ''' <returns>入力 <paramref name="value"/> の逆数です。</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class