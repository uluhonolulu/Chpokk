Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Shapes
Imports System.Windows.Threading

Namespace Controls
    ''' <summary>
    ''' Control que proporciona un indicador visual cuando una aplicación está ocupada.
    ''' </summary>
    <TemplateVisualState(Name:=VisualStates.StateIdle, GroupName:=VisualStates.GroupBusyStatus)> _
    <TemplateVisualState(Name:=VisualStates.StateBusy, GroupName:=VisualStates.GroupBusyStatus)> _
    <TemplateVisualState(Name:=VisualStates.StateVisible, GroupName:=VisualStates.GroupVisibility)> _
    <TemplateVisualState(Name:=VisualStates.StateHidden, GroupName:=VisualStates.GroupVisibility)> _
    <StyleTypedProperty([Property]:="OverlayStyle", StyleTargetType:=GetType(Rectangle))> _
    <StyleTypedProperty([Property]:="ProgressBarStyle", StyleTargetType:=GetType(ProgressBar))> _
    Public Class BusyIndicator
        Inherits ContentControl
        ''' <summary>
        ''' Obtiene o establece un valor que indica si el BusyContent es visible.
        ''' </summary>
        Protected Property IsContentVisible() As Boolean

        ''' <summary>
        ''' Timer utilizado para retrasar la visualización inicial y evitar el parpadeo.
        ''' </summary>
        Private _displayAfterTimer As DispatcherTimer

        ''' <summary>
        ''' Crea una instancia de una nueva instancia del control BusyIndicator.
        ''' </summary>
        Public Sub New()
            DefaultStyleKey = GetType(BusyIndicator)
            _displayAfterTimer = New DispatcherTimer()
            AddHandler _displayAfterTimer.Tick, AddressOf DisplayAfterTimerElapsed
        End Sub

        ''' <summary>
        ''' Invalida el método OnApplyTemplate.
        ''' </summary>
        Public Overloads Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            ChangeVisualState(False)
        End Sub

        ''' <summary>
        ''' Controlador para el DisplayAfterTimer.
        ''' </summary>
        ''' <param name="sender">Remitente del evento.</param>
        ''' <param name="e">Argumentos del evento.</param>
        Private Sub DisplayAfterTimerElapsed(ByVal sender As Object, ByVal e As EventArgs)
            _displayAfterTimer.Stop()
            IsContentVisible = True
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' Cambia los estados visuales del control.
        ''' </summary>
        ''' <param name="useTransitions">True si se tienen que utilizar transiciones de estado.</param>
        Protected Overridable Sub ChangeVisualState(ByVal useTransitions As Boolean)
            VisualStateManager.GoToState(Me, If(IsBusy, VisualStates.StateBusy, VisualStates.StateIdle), useTransitions)
            VisualStateManager.GoToState(Me, If(IsContentVisible, VisualStates.StateVisible, VisualStates.StateHidden), useTransitions)
        End Sub

        ''' <summary>
        ''' Obtiene o establece un valor que indica si se tiene que mostrar el indicador de ocupado.
        ''' </summary>
        Public Property IsBusy() As Boolean
            Get
                Return CBool(GetValue(IsBusyProperty))
            End Get
            Set(ByVal value As Boolean)
                SetValue(IsBusyProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifica la dependency property IsBusy.
        ''' </summary>
        Public Shared ReadOnly IsBusyProperty As DependencyProperty = DependencyProperty.Register( _
            "IsBusy", _
            GetType(Boolean), _
            GetType(BusyIndicator), _
            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsBusyChanged)))

        ''' <summary>
        ''' La propiedad IsBusyProperty cambió de controlador.
        ''' </summary>
        ''' <param name="d">BusyIndicator que cambió su IsBusy.</param>
        ''' <param name="e">Argumentos del evento.</param>
        Private Shared Sub OnIsBusyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            DirectCast(d, BusyIndicator).OnIsBusyChanged(e)
        End Sub

        ''' <summary>
        ''' La propiedad IsBusyProperty cambió de controlador.
        ''' </summary>
        ''' <param name="e">Argumentos de Event.</param>
        Protected Overridable Sub OnIsBusyChanged(ByVal e As DependencyPropertyChangedEventArgs)
            If IsBusy Then
                If DisplayAfter.Equals(TimeSpan.Zero) Then
                    ' Hacer visible
                    IsContentVisible = True
                Else
                    ' Establecer la visibilidad de un temporizador
                    _displayAfterTimer.Interval = DisplayAfter
                    _displayAfterTimer.Start()
                End If
            Else
                ' Anular visibilidad
                _displayAfterTimer.Stop()
                IsContentVisible = False
            End If
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' Obtiene o establece un valor que indica el contenido ocupado que se va a mostrar al usuario.
        ''' </summary>
        Public Property BusyContent() As Object
            Get
                Return DirectCast(GetValue(BusyContentProperty), Object)
            End Get
            Set(ByVal value As Object)
                SetValue(BusyContentProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifica la dependency property BusyContent.
        ''' </summary>
        Public Shared ReadOnly BusyContentProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContent", _
            GetType(Object), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Obtiene o establece un valor que indica la plantilla que se va a utilizar para mostrar el contenido ocupado al usuario.
        ''' </summary>
        Public Property BusyContentTemplate() As DataTemplate
            Get
                Return DirectCast(GetValue(BusyContentTemplateProperty), DataTemplate)
            End Get
            Set(ByVal value As DataTemplate)
                SetValue(BusyContentTemplateProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifica la dependency property BusyTemplate.
        ''' </summary>
        Public Shared ReadOnly BusyContentTemplateProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContentTemplate", _
            GetType(DataTemplate), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Obtiene o establece un valor que indica cuánto tiempo pasará hasta que se muestre el contenido ocupado.
        ''' </summary>
        Public Property DisplayAfter() As TimeSpan
            Get
                Return DirectCast(GetValue(DisplayAfterProperty), TimeSpan)
            End Get
            Set(ByVal value As TimeSpan)
                SetValue(DisplayAfterProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifica la dependency property DisplayAfter.
        ''' </summary>
        Public Shared ReadOnly DisplayAfterProperty As DependencyProperty = DependencyProperty.Register( _
            "DisplayAfter", _
            GetType(TimeSpan), _
            GetType(BusyIndicator), _
            New PropertyMetadata(TimeSpan.FromSeconds(0.1)))

        ''' <summary>
        ''' Obtiene o establece un valor que indica el estilo que se va a utilizar para la superposición.
        ''' </summary>
        Public Property OverlayStyle() As Style
            Get
                Return DirectCast(GetValue(OverlayStyleProperty), Style)
            End Get
            Set(ByVal value As Style)
                SetValue(OverlayStyleProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifica la dependency property OverlayStyle.
        ''' </summary>
        Public Shared ReadOnly OverlayStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "OverlayStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Obtiene o establece un valor que indica el estilo que se va a utilizar para la barra de progreso.
        ''' </summary>
        Public Property ProgressBarStyle() As Style
            Get
                Return DirectCast(GetValue(ProgressBarStyleProperty), Style)
            End Get
            Set(ByVal value As Style)
                SetValue(ProgressBarStyleProperty, value)
            End Set
        End Property

        ''' <summary>
        ''' Identifica la dependency property ProgressBarStyle.
        ''' </summary>
        Public Shared ReadOnly ProgressBarStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "ProgressBarStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))
    End Class
End Namespace