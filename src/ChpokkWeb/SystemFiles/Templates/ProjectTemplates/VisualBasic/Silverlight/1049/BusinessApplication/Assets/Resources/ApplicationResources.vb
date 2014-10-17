Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Browser


''' <summary>
''' Обертка доступа к классам ресурсов со строгой типизацией, чтобы можно было привязывать свойства элементов управления к строкам ресурсов в XAML.
''' </summary>
Public NotInheritable Class ApplicationResources
    Private Shared _applicationStrings As New ApplicationStrings()
    Private Shared _errorResources As New ErrorResources()

    ''' <summary>
    ''' Возвращает <see cref="ApplicationStrings"/>.
    ''' </summary>
    Public ReadOnly Property Strings() As ApplicationStrings
        Get
            Return _applicationStrings
        End Get
    End Property

    ''' <summary>
    ''' Возвращает <see cref="ErrorResources"/>.
    ''' </summary>
    Public ReadOnly Property Errors() As ErrorResources
        Get
            Return _errorResources
        End Get
    End Property
End Class
