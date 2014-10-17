Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports Microsoft.VisualStudio.Text
Imports Microsoft.VisualStudio.Text.Editor
Imports Microsoft.VisualStudio.Text.Formatting

''' <summary>
''' $safeprojectname$ places red boxes behind all the "a"s in the editor window
''' </summary>
Class $safeprojectname$

    Private WithEvents _view As IWpfTextView
    Private ReadOnly _layer As IAdornmentLayer
    Private ReadOnly _brush As Brush
    Private ReadOnly _pen As Pen

    Public Sub New(ByVal view As IWpfTextView)
        _view = view
        _layer = view.GetAdornmentLayer("$safeprojectname$")

        'Create the pen and brush to color the box behind the a's
        Dim brush As New SolidColorBrush(Color.FromArgb(&H20, &H0, &H0, &HFF))
        brush.Freeze()
        Dim penBrush As New SolidColorBrush(Colors.Red)
        penBrush.Freeze()
        Dim pen As New Pen(penBrush, 0.5)
        pen.Freeze()

        _brush = brush
        _pen = pen
    End Sub

    ''' <summary>
    ''' On layout change add the adornment to any reformatted lines
    ''' </summary>
    Private Sub OnLayoutChanged(ByVal sender As Object, ByVal e As TextViewLayoutChangedEventArgs) Handles _view.LayoutChanged
        For Each line In e.NewOrReformattedLines
            Me.CreateVisuals(line)
        Next line
    End Sub

    ''' <summary>
    ''' Within the given line add the scarlet box behind the a
    ''' </summary>
    Private Sub CreateVisuals(ByVal line As ITextViewLine)
        'grab a reference to the lines in the current TextView 
        Dim textViewLines = _view.TextViewLines
        Dim lineStart As Integer = line.Start
        Dim lineEnd As Integer = line.End

        'Loop through each character, and place a box around any a 
        For i = lineStart To lineEnd - 1
            If _view.TextSnapshot(i) = "a"c Then
                Dim charSpan As New SnapshotSpan(_view.TextSnapshot, Span.FromBounds(i, i + 1))
                Dim g As Geometry = textViewLines.GetMarkerGeometry(charSpan)
                If g IsNot Nothing Then
                    Dim drawing As New GeometryDrawing(_brush, _pen, g)
                    drawing.Freeze()

                    Dim drawingImage As New DrawingImage(drawing)
                    drawingImage.Freeze()

                    Dim image As New Image()
                    image.Source = drawingImage

                    'Align the image with the top of the bounds of the text geometry
                    Canvas.SetLeft(image, g.Bounds.Left)
                    Canvas.SetTop(image, g.Bounds.Top)

                    _layer.AddAdornment(AdornmentPositioningBehavior.TextRelative, charSpan, Nothing, image, Nothing)
                End If
            End If
        Next
    End Sub
    
End Class
