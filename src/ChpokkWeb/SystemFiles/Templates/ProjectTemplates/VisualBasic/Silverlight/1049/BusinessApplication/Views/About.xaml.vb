Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Класс <see cref="Page"/> представляет сведения о текущем приложении.
''' </summary>
Partial Public Class About
    Inherits Page

    ''' <summary>
    ''' Создает новый экземпляр класса <see cref="About"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.AboutPageTitle
    End Sub

    ''' <summary>
    ''' Выполняется, когда пользователь переходит на эту страницу.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class