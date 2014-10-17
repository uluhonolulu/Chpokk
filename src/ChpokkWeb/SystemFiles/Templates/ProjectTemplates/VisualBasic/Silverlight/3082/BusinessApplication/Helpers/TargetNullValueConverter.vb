Imports System
Imports System.Windows.Data

''' <summary>
''' IValueConverter bidireccional que permite enlazar una propiedad de un objeto enlazable que puede ser un valor de string vacía a una propiedad de dependencia que en ese caso debería establecerse en null.
''' </summary>
Public Class TargetNullValueConverter
    Implements IValueConverter

    ''' <summary>
    ''' Convierte strings <c>null</c> o vacías en <c>null</c>.
    ''' </summary>
    ''' <param name="value">Valor que se va a convertir.</param>
    ''' <param name="targetType">Es el type esperado del resultado (omitido).</param>
    ''' <param name="parameter">Parámetro opcional (omitido).</param>
    ''' <param name="culture">Referencia cultural para la conversión (omitida).</param>
    ''' <returns>Si el <paramref name="value"/> es <c>null</c> o está vacío, este método devuelve <c>null</c>, de lo contrario, devuelve el <paramref name="value"/>.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim strValue As String = Nothing

        If TypeOf value Is String Then
            strValue = DirectCast(value, String)
        End If

        Return If(String.IsNullOrEmpty(strValue), Nothing, value)
    End Function

    ''' <summary>
    ''' Revierte <c>null</c> a <see cref="String.Empty"/>.
    ''' </summary>
    ''' <param name="value">Valor que se va a convertir.</param>
    ''' <param name="targetType">Es el type esperado del resultado (omitido).</param>
    ''' <param name="parameter">Parámetro opcional (omitido).</param>
    ''' <param name="culture">Referencia cultural para la conversión (omitida).</param>
    ''' <returns>Si <paramref name="value"/> es <c>null</c>, devuelve <see cref="String.Empty"/>, de lo contrario, <paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value Is Nothing, String.Empty, value)
    End Function
End Class