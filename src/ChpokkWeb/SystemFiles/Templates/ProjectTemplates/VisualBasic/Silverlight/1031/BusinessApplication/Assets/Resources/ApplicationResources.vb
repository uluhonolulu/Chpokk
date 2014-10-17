Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Browser


''' <summary>
''' Umschließt den Zugriff auf stark typisierte Ressourcenklassen, damit Steuerelementeigenschaften an Ressourcenzeichenfolgen in XAML gebunden werden können.
''' </summary>
Public NotInheritable Class ApplicationResources
    Private Shared _applicationStrings As New ApplicationStrings()
    Private Shared _errorResources As New ErrorResources()

    ''' <summary>
    ''' Ruft die <see cref="ApplicationStrings"/> ab.
    ''' </summary>
    Public ReadOnly Property Strings() As ApplicationStrings
        Get
            Return _applicationStrings
        End Get
    End Property

    ''' <summary>
    ''' Ruft die <see cref="ErrorResources"/> ab.
    ''' </summary>
    Public ReadOnly Property Errors() As ErrorResources
        Get
            Return _errorResources
        End Get
    End Property
End Class
