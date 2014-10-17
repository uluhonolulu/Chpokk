Imports System.Windows.Controls
Imports System.Windows.Navigation

''' <summary>
''' Домашняя страница приложения.
''' </summary>
Partial Public Class Home
    Inherits Page

    ''' <summary>
    ''' Создает новый экземпляр класса <see cref="Home"/>.
    ''' </summary>
    Public Sub New()
        InitializeComponent()

        Me.Title = ApplicationStrings.HomePageTitle
    End Sub

    ''' <summary>
    ''' Выполняется, когда пользователь переходит на эту страницу.
    ''' </summary>
    Protected Overloads Overrides Sub OnNavigatedTo(ByVal e As NavigationEventArgs)
    End Sub
End Class