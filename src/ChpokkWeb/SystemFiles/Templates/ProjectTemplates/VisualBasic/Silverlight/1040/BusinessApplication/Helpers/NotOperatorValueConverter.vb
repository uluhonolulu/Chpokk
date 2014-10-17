Imports System
Imports System.Windows.Data

''' <summary>
''' Oggetto <see cref="IValueConverter"/> bidirezionale che consente di associare l'inverso di una proprietà booleana a una proprietà di dipendenza.
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' Converte l'oggetto <paramref name="value"/> specificato nel relativo inverso.
    ''' </summary>
    ''' <param name="value">Valore <c>Boolean</c> da convertire.</param>
    ''' <param name="targetType">Tipo in cui convertire (ignorato).</param>
    ''' <param name="parameter">Parametro facoltativo (ignorato).</param>
    ''' <param name="culture">Impostazioni cultura della conversione (ignorate).</param>
    ''' <returns>Inverso dell'oggetto <paramref name="value"/> di input.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' Inverso dell'oggetto <see cref="Convert"/>.
    ''' </summary>
    ''' <param name="value">Valore da riconvertire.</param>
    ''' <param name="targetType">Tipo in cui convertire (ignorato).</param>
    ''' <param name="parameter">Parametro facoltativo (ignorato).</param>
    ''' <param name="culture">Impostazioni cultura della conversione (ignorate).</param>
    ''' <returns>Inverso dell'oggetto <paramref name="value"/> di input.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class