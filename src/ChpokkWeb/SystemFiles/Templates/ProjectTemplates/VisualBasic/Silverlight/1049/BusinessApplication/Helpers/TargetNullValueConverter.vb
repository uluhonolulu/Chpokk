Imports System
Imports System.Windows.Data

''' <summary>
''' Двусторонний IValueConverter, который позволяет привязывать свойство привязываемого объекта, которое может быть пустой строкой (string), к свойству зависимости (dependency property), которое должно быть в данном случае установлено в значение null.
''' </summary>
Public Class TargetNullValueConverter
    Implements IValueConverter

    ''' <summary>
    ''' Преобразует значения <c>null</c> и пустые строки (strings) в значения <c>null</c>.
    ''' </summary>
    ''' <param name="value">Значение для преобразования.</param>
    ''' <param name="targetType">Ожидаемый тип (type) преобразования (не используется).</param>
    ''' <param name="parameter">Необязательный параметр (не используется).</param>
    ''' <param name="culture">Культура преобразования (не используется).</param>
    ''' <returns>Если <paramref name="value"/> имеет значение <c>null</c> или пуст, этот метод возвращает значение <c>null</c>. В противном случае возвращается значение <paramref name="value"/>.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim strValue As String = Nothing

        If TypeOf value Is String Then
            strValue = DirectCast(value, String)
        End If

        Return If(String.IsNullOrEmpty(strValue), Nothing, value)
    End Function

    ''' <summary>
    ''' Преобразует значение <c>null</c> обратно в <see cref="String.Empty"/>.
    ''' </summary>
    ''' <param name="value">Значение для преобразования.</param>
    ''' <param name="targetType">Ожидаемый тип (type) преобразования (не используется).</param>
    ''' <param name="parameter">Необязательный параметр (не используется).</param>
    ''' <param name="culture">Культура преобразования (не используется).</param>
    ''' <returns>Если <paramref name="value"/> имеет значение <c>null</c>, возвращается <see cref="String.Empty"/>. В противном случае возвращается <paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value Is Nothing, String.Empty, value)
    End Function
End Class