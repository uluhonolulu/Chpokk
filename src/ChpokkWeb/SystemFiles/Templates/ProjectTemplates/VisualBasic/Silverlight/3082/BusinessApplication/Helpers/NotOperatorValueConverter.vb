Imports System
Imports System.Windows.Data

''' <summary>
''' <see cref="IValueConverter"/> bidireccional que permite enlazar el inverso de una propiedad boolean a una dependency property.
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' Convierte el <paramref name="value"/> proporcionado en su inverso.
    ''' </summary>
    ''' <param name="value">Valor <c>Boolean</c> que se va a convertir.</param>
    ''' <param name="targetType">Es el type al que se va a convertir (omitido).</param>
    ''' <param name="parameter">Parámetro opcional (omitido).</param>
    ''' <param name="culture">Referencia cultural de la conversión (omitida).</param>
    ''' <returns>Inverso del <paramref name="value"/> de entrada.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' Inverso de <see cref="Convert"/>.
    ''' </summary>
    ''' <param name="value">Valor que se va a revertir.</param>
    ''' <param name="targetType">Es el type al que se va a convertir (omitido).</param>
    ''' <param name="parameter">Parámetro opcional (omitido).</param>
    ''' <param name="culture">Referencia cultural de la conversión (omitida).</param>
    ''' <returns>Inverso del <paramref name="value"/> de entrada.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class