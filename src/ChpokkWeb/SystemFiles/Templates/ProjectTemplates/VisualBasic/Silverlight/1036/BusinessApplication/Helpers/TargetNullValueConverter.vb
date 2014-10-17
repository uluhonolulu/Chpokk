Imports System
Imports System.Windows.Data

''' <summary>
''' IValueConverter bidirectionnel qui vous permet de lier une propriété sur un objet pouvant être lié qui peut être une valeur string vide à une propriété de dépendance à laquelle la valeur null doit être affectée dans ce cas.
''' </summary>
Public Class TargetNullValueConverter
    Implements IValueConverter

    ''' <summary>
    ''' Convertit les strings <c>null</c> ou vides en <c>null</c>.
    ''' </summary>
    ''' <param name="value">Valeur à convertir.</param>
    ''' <param name="targetType">Type attendu du résultat (ignoré).</param>
    ''' <param name="parameter">Paramètre optionnel (ignoré).</param>
    ''' <param name="culture">Culture de la conversion (ignorée).</param>
    ''' <returns>Si <paramref name="value"/> a la valeur <c>null</c> ou est vide, cette méthode retourne <c>null</c> ; sinon, elle retourne <paramref name="value"/>.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Dim strValue As String = Nothing

        If TypeOf value Is String Then
            strValue = DirectCast(value, String)
        End If

        Return If(String.IsNullOrEmpty(strValue), Nothing, value)
    End Function

    ''' <summary>
    ''' Reconvertit <c>null</c> en <see cref="String.Empty"/>.
    ''' </summary>
    ''' <param name="value">Valeur à convertir.</param>
    ''' <param name="targetType">Type attendu du résultat (ignoré).</param>
    ''' <param name="parameter">Paramètre optionnel (ignoré).</param>
    ''' <param name="culture">Culture de la conversion (ignorée).</param>
    ''' <returns>Si <paramref name="value"/> a la valeur <c>null</c>, <see cref="String.Empty"/> est retourné ; sinon, <paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return If(value Is Nothing, String.Empty, value)
    End Function
End Class