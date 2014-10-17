Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Browser


''' <summary>
''' Encapsula el acceso a las clases de recursos fuertemente tipadas de modo que se puedan enlazar las propiedades del control a las cadenas de recursos del XAML.
''' </summary>
Public NotInheritable Class ApplicationResources
    Private Shared _applicationStrings As New ApplicationStrings()
    Private Shared _errorResources As New ErrorResources()

    ''' <summary>
    ''' Obtiene las <see cref="ApplicationStrings"/>.
    ''' </summary>
    Public ReadOnly Property Strings() As ApplicationStrings
        Get
            Return _applicationStrings
        End Get
    End Property

    ''' <summary>
    ''' Obtiene los <see cref="ErrorResources"/>.
    ''' </summary>
    Public ReadOnly Property Errors() As ErrorResources
        Get
            Return _errorResources
        End Get
    End Property
End Class
