Imports System
Imports System.Windows.Data

''' <summary>
''' Oggetto IValueConverter bidirezionale che consente di associare una proprietà di un oggetto associabile che può essere un valore di stringa vuoto a una proprietà di dipendenza che in tal caso deve essere impostata su null.
''' </summary>
Public Class TargetNullValueConverter
    Implements IValueConverter

    ''' <summary>
    ''' Converte <c>null</c> o strings vuote in <c>null</c>.
    ''' </summary>
    ''' <param name="value">Valore da convertire.</param>
    ''' <param name="targetType">Tipo previsto del risultato (ignorato).</param>
    ''' <param name="parameter">Parametro facoltativo (ignorato).</param>
    ''' <param name="culture">Impostazioni cultura per la conversione (ignorate).</param>
    ''' <returns>Se <paramref name="value"/> è <c>null</c> o vuoto, questo metodo restituisce <c>null</c>; in caso contrario, restituisce <paramref name="value"/>.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim strValue As String = Nothing

        If TypeOf value Is String Then
            strValue = DirectCast(value, String)
        End If

        Return If(String.IsNullOrEmpty(strValue), Nothing, value)
    End Function

    ''' <summary>
    ''' Riconverte <c>null</c> in <see cref="String.Empty"/>.
    ''' </summary>
    ''' <param name="value">Valore da convertire.</param>
    ''' <param name="targetType">Tipo previsto del risultato (ignorato).</param>
    ''' <param name="parameter">Parametro facoltativo (ignorato).</param>
    ''' <param name="culture">Impostazioni cultura per la conversione (ignorate).</param>
    ''' <returns>Se <paramref name="value"/> è <c>null</c>, restituisce <see cref="String.Empty"/>; in caso contrario, <paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value Is Nothing, String.Empty, value)
    End Function
End Class