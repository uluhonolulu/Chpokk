Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Browser


''' <summary>
''' Esegue il wrapping dell'accesso alle classi di risorse fortemente tipizzate in modo da poter associare le proprietà dei controllo alle stringhe di risorse in XAML.
''' </summary>
Public NotInheritable Class ApplicationResources
    Private Shared _applicationStrings As New ApplicationStrings()
    Private Shared _errorResources As New ErrorResources()

    ''' <summary>
    ''' Ottiene l'oggetto <see cref="ApplicationStrings"/>.
    ''' </summary>
    Public ReadOnly Property Strings() As ApplicationStrings
        Get
            Return _applicationStrings
        End Get
    End Property

    ''' <summary>
    ''' Ottiene l'oggetto <see cref="ErrorResources"/>.
    ''' </summary>
    Public ReadOnly Property Errors() As ErrorResources
        Get
            Return _errorResources
        End Get
    End Property
End Class
