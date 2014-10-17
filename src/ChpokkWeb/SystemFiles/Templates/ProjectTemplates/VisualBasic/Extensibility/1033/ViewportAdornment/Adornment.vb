Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media
Imports Microsoft.VisualStudio.Text.Editor

''' <summary>
''' Adornment class that draws a square box in the top right hand corner of the viewport
''' </summary>
Class $safeprojectname$

    Private WithEvents _view As IWpfTextView
    Private ReadOnly _image As Image
    Private ReadOnly _adornmentLayer As IAdornmentLayer

    ''' <summary>
    ''' Creates a square image and attaches an event handler to the layout changed event that
    ''' adds the the square in the upper right-hand corner of the TextView via the adornment layer
    ''' </summary>
    ''' <param name="view">The <see cref="IWpfTextView"/> upon which the adornment will be drawn</param>
    Public Sub New(ByVal view As IWpfTextView)

        _view = view

        'Set the fill color of the square
        Dim brush As New SolidColorBrush(Colors.BlueViolet)
        brush.Freeze()
        
        'Set the outline color of the square
        Dim penBrush As New SolidColorBrush(Colors.Red)
        penBrush.Freeze()
        Dim pen As New Pen(penBrush, 0.5)
        pen.Freeze()

        'draw a square with the created brush and pen
        Dim r As New Rect(0, 0, 30, 30)
        Dim g As New RectangleGeometry(r)
        Dim drawing As New GeometryDrawing(brush, pen, g)
        drawing.Freeze()

        Dim drawingImage As New DrawingImage(drawing)
        drawingImage.Freeze()

        _image = New Image()
        _image.Source = drawingImage

        'Grab a reference to the adornment layer that this adornment should be added to
        _adornmentLayer = view.GetAdornmentLayer("$safeprojectname$")

    End Sub

    Private Sub OnSizeChange() Handles _view.ViewportHeightChanged, _view.ViewportWidthChanged
    
            'clear the adornment layer of previous adornments
            _adornmentLayer.RemoveAllAdornments()

            'Place the image in the top right hand corner of the Viewport
            Canvas.SetLeft(_image, _view.ViewportRight - 60)
            Canvas.SetTop(_image, _view.ViewportTop + 30)

            'add the image to the adornment layer and make it relative to the viewport
            _adornmentLayer.AddAdornment(AdornmentPositioningBehavior.ViewportRelative, Nothing, Nothing, _image, Nothing)
            
    End Sub

End Class
