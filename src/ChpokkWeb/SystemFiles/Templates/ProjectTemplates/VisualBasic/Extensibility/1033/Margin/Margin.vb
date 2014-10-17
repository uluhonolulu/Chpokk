Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports Microsoft.VisualStudio.Text.Editor

''' <summary>
''' A class detailing the margin's visual definition including both size and content.
''' </summary>
Class $safeprojectname$
    Inherits Canvas
    Implements IWpfTextViewMargin

    'The name of the margin
    Public Const MarginName As String = "$safeprojectname$"

    ' The IWpfTextView that our margin will be attached to.
    Private _textView As IWpfTextView

    ' A flag stating whether this margin has been disposed
    Private _isDisposed As Boolean

    ''' <summary>
    ''' Creates a <see cref="$safeprojectname$"/> for a given <see cref="IWpfTextView"/>.
    ''' </summary>
    ''' <param name="textView">The <see cref="IWpfTextView"/> to attach the margin to.</param>
    Public Sub New(ByVal textView As IWpfTextView)
    
        ' Set the IWpfTextView
        _textView = textView

        ' Establish the background of the margin.
        Me.Height = 20
        Me.ClipToBounds = True
        Me.Background = New SolidColorBrush(Colors.LightGreen)

        'add a green colored label that says "Hello World!"
        Dim label As New Label With {
            .Background = New SolidColorBrush(Colors.LightGreen),
            .Content = "$labelContent$"
        }
        Me.Children.Add(label)

    End Sub

    Private Sub ThrowIfDisposed()
        If _isDisposed Then
            Throw New ObjectDisposedException(MarginName)
        End If
    End Sub

    #Region "IWpfTextViewMargin Members"

    ''' <summary>
    ''' The <see cref="Sytem.Windows.FrameworkElement"/> that implements the visual representation
    ''' of the margin.
    ''' </summary>
    Public ReadOnly Property VisualElement() As FrameworkElement Implements IWpfTextViewMargin.VisualElement
        ' Since this margin implements Canvas, this is the object which renders
        ' the margin.
        Get
            ThrowIfDisposed()
            Return Me
        End Get
    End Property

    #End Region

    #Region "ITextViewMargin Members"

    Public ReadOnly Property MarginSize() As Double Implements IWpfTextViewMargin.MarginSize
        ' Since this is a horizontal margin, its width will be bound to the width of the text view.
        ' Therefore, its size is its height.
        Get
            ThrowIfDisposed()
            Return Me.ActualHeight
        End Get
    End Property

    Public ReadOnly Property Enabled() As Boolean Implements IWpfTextViewMargin.Enabled
        'The margin should always be enabled
        Get
            ThrowIfDisposed()
            Return True
        End Get
    End Property

    ''' <summary>
    ''' Returns an instance of the margin if this is the margin that has been requested.
    ''' </summary>
    ''' <param name="marginName">The name of the margin requested</param>
    ''' <returns>An instance of $safeprojectname$ or null</returns>
    Public Function GetTextViewMargin(ByVal marginName As String) As ITextViewMargin Implements IWpfTextViewMargin.GetTextViewMargin
        Return If(marginName = $safeprojectname$.MarginName, Me, Nothing)
    End Function

    'dispose of the margin
    Public Sub Dispose() Implements IDisposable.Dispose
        If Not _isDisposed Then
            GC.SuppressFinalize(Me)
            _isDisposed = True
        End If
    End Sub
    #End Region
    
End Class
