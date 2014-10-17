Imports System
Imports System.Windows.Data

''' <summary>
''' Двусторонний <see cref="IValueConverter"/>, который позволяет привязывать отрицание логического (boolean) свойства к свойству зависимости (dependency property).
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' Преобразует данное значение <paramref name="value"/> в его отрицание.
    ''' </summary>
    ''' <param name="value">Значение типа <c>Boolean</c> для преобразования.</param>
    ''' <param name="targetType">Преобразуемый тип (type) (не используется).</param>
    ''' <param name="parameter">Необязательный параметр (не используется).</param>
    ''' <param name="culture">Культура преобразования (не используется).</param>
    ''' <returns>Отрицание входного параметра <paramref name="value"/>.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' Отрицание <see cref="Convert"/>.
    ''' </summary>
    ''' <param name="value">Значение для обратного преобразования.</param>
    ''' <param name="targetType">Преобразуемый тип (type) (не используется).</param>
    ''' <param name="parameter">Необязательный параметр (не используется).</param>
    ''' <param name="culture">Культура преобразования (не используется).</param>
    ''' <returns>Отрицание входного параметра <paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class