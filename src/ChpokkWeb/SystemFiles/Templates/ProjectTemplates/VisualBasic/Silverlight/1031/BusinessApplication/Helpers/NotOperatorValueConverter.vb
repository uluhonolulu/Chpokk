Imports System
Imports System.Windows.Data

''' <summary>
''' Zweiwege-<see cref="IValueConverter"/>, durch den die Inversion einer "boolean"-Eigenschaft an eine Abhängigkeitseigenschaft gebunden werden kann.
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' Konvertiert den angegebenen <paramref name="value"/> in sein Gegenteil (Inversion).
    ''' </summary>
    ''' <param name="value">Der <c>Boolean</c> Wert, der konvertiert werden soll.</param>
    ''' <param name="targetType">Der "type", in den konvertiert werden soll (wird ignoriert).</param>
    ''' <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
    ''' <param name="culture">Die Kultur der Konversion (wird ignoriert).</param>
    ''' <returns>Die Inversion des Eingabe-<paramref name="value"/>.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' Die Inversion der <see cref="Convert"/>.
    ''' </summary>
    ''' <param name="value">Der zurückzukonvertierende Wert.</param>
    ''' <param name="targetType">Der "type", in den konvertiert werden soll (wird ignoriert).</param>
    ''' <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
    ''' <param name="culture">Die Kultur der Konversion (wird ignoriert).</param>
    ''' <returns>Die Inversion des Eingabe-<paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class