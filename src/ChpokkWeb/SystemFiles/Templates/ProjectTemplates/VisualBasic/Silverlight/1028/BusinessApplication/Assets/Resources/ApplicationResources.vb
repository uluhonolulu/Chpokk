Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Windows.Browser


''' <summary>
''' 將存取包裝到強型別資源類別，就可以將控制項屬性繫結到 XAML 中的資源字串。
''' </summary>
Public NotInheritable Class ApplicationResources
    Private Shared _applicationStrings As New ApplicationStrings()
    Private Shared _errorResources As New ErrorResources()

    ''' <summary>
    ''' 取得 <see cref="ApplicationStrings"/>。
    ''' </summary>
    Public ReadOnly Property Strings() As ApplicationStrings
        Get
            Return _applicationStrings
        End Get
    End Property

    ''' <summary>
    ''' 取得 <see cref="ErrorResources"/>。
    ''' </summary>
    Public ReadOnly Property Errors() As ErrorResources
        Get
            Return _errorResources
        End Get
    End Property
End Class
