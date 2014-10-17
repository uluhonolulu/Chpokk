Imports System
Imports System.Windows.Data

''' <summary>
''' Zweiwege-IValueConverter, durch den eine Eigenschaft an ein bindbares Objekt gebunden werden kann, das aus einem leeren string-Wert für eine Abhängigkeitseigenschaft bestehen kann, die in diesem Fall auf null festgelegt werden sollte.
''' </summary>
Public Class TargetNullValueConverter
    Implements IValueConverter

    ''' <summary>
    ''' Konvertiert <c>null</c> oder leere strings zu <c>null</c>.
    ''' </summary>
    ''' <param name="value">Der zu konvertierende Wert.</param>
    ''' <param name="targetType">Der erwartete "type" des Ergebnisses (wird ignoriert).</param>
    ''' <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
    ''' <param name="culture">Die Kultur für die Konversion (wird ignoriert).</param>
    ''' <returns>Wenn <paramref name="value"/> <c>null</c> oder leer ist, gibt diese Methode <c>null</c> zurück. Andernfalls wird der <paramref name="value"/> zurückgegeben.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim strValue As String = Nothing

        If TypeOf value Is String Then
            strValue = DirectCast(value, String)
        End If

        Return If(String.IsNullOrEmpty(strValue), Nothing, value)
    End Function

    ''' <summary>
    ''' Konvertiert <c>null</c> in <see cref="String.Empty"/> zurück.
    ''' </summary>
    ''' <param name="value">Der zu konvertierende Wert.</param>
    ''' <param name="targetType">Der erwartete "type" des Ergebnisses (wird ignoriert).</param>
    ''' <param name="parameter">Optionaler Parameter (wird ignoriert).</param>
    ''' <param name="culture">Die Kultur für die Konversion (wird ignoriert).</param>
    ''' <returns>Falls <paramref name="value"/> <c>null</c> ist, wird <see cref="String.Empty"/> zurückgegeben, andernfalls <paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value Is Nothing, String.Empty, value)
    End Function
End Class