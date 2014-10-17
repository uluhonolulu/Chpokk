Imports System
Imports System.Windows.Data

''' <summary>
''' <see cref="IValueConverter"/> bidirectionnel qui vous permet de lier l'inverse d'une propriété boolean à une propriété de dépendance.
''' </summary>
Public Class NotOperatorValueConverter
    Implements IValueConverter


    ''' <summary>
    ''' Convertit le <paramref name="value"/> donné pour qu'il soit son inverse.
    ''' </summary>
    ''' <param name="value">Valeur <c>Boolean</c> à convertir.</param>
    ''' <param name="targetType">Type dans lequel effectuer la conversion (ignoré).</param>
    ''' <param name="parameter">Paramètre optionnel (ignoré).</param>
    ''' <param name="culture">Culture de la conversion (ignorée).</param>
    ''' <returns>Inverse de l'entrée <paramref name="value"/>.</returns>
    Public Function Convert(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.Convert
        Return Not DirectCast(value, Boolean)
    End Function

    ''' <summary>
    ''' Inverse de <see cref="Convert"/>.
    ''' </summary>
    ''' <param name="value">Valeur vers laquelle effectuer la reconversion.</param>
    ''' <param name="targetType">Type dans lequel effectuer la conversion (ignoré).</param>
    ''' <param name="parameter">Paramètre optionnel (ignoré).</param>
    ''' <param name="culture">Culture de la conversion (ignorée).</param>
    ''' <returns>Inverse de l'entrée <paramref name="value"/>.</returns>
    Public Function ConvertBack(ByVal value As Object, ByVal targetType As Type, ByVal parameter As Object, ByVal culture As System.Globalization.CultureInfo) As Object Implements IValueConverter.ConvertBack
        Return Not DirectCast(value, Boolean)
    End Function
End Class