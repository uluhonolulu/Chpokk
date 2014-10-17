Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Shapes
Imports System.Windows.Threading

Namespace Controls
    ''' <summary>
    ''' Элемент управления, отображающий занятость приложения.
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
        ''' Возвращает или задает значение, указывающее, отображается ли BusyContent.
        ''' </summary>
        Protected Property IsContentVisible() As Boolean

        ''' <summary>
        ''' Таймер (Timer) позволяет отложить первоначальное отображение и избежать мигания.
        ''' </summary>
        Private _displayAfterTimer As DispatcherTimer

        ''' <summary>
        ''' Создает экземпляр элемента управления BusyIndicator.
        ''' </summary>
        Public Sub New()
            DefaultStyleKey = GetType(BusyIndicator)
            _displayAfterTimer = New DispatcherTimer()
            AddHandler _displayAfterTimer.Tick, AddressOf DisplayAfterTimerElapsed
        End Sub

        ''' <summary>
        ''' Переопределяет метод OnApplyTemplate.
        ''' </summary>
        Public Overloads Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            ChangeVisualState(False)
        End Sub

        ''' <summary>
        ''' Обработчик события DisplayAfterTimer.
        ''' </summary>
        ''' <param name="sender">Отправитель события.</param>
        ''' <param name="e">Аргументы события.</param>
        Private Sub DisplayAfterTimerElapsed(ByVal sender As Object, ByVal e As EventArgs)
            _displayAfterTimer.Stop()
            IsContentVisible = True
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' Изменяет визуальные состояния элементов управления.
        ''' </summary>
        ''' <param name="useTransitions">Значение True, если должны использоваться переходы состояния.</param>
        Protected Overridable Sub ChangeVisualState(ByVal useTransitions As Boolean)
            VisualStateManager.GoToState(Me, If(IsBusy, VisualStates.StateBusy, VisualStates.StateIdle), useTransitions)
            VisualStateManager.GoToState(Me, If(IsContentVisible, VisualStates.StateVisible, VisualStates.StateHidden), useTransitions)
        End Sub

        ''' <summary>
        ''' Возвращает или задает значение, указывающее, должен ли отображаться индикатор занятости.
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
        ''' Указывает свойство зависимости (dependency property) IsBusy.
        ''' </summary>
        Public Shared ReadOnly IsBusyProperty As DependencyProperty = DependencyProperty.Register( _
            "IsBusy", _
            GetType(Boolean), _
            GetType(BusyIndicator), _
            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsBusyChanged)))

        ''' <summary>
        ''' Обработчик изменения свойства IsBusyProperty.
        ''' </summary>
        ''' <param name="d">Объект BusyIndicator, который изменяет свое состояние IsBusy.</param>
        ''' <param name="e">Аргументы события.</param>
        Private Shared Sub OnIsBusyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            DirectCast(d, BusyIndicator).OnIsBusyChanged(e)
        End Sub

        ''' <summary>
        ''' Обработчик изменения свойства IsBusyProperty.
        ''' </summary>
        ''' <param name="e">Аргументы события (Event).</param>
        Protected Overridable Sub OnIsBusyChanged(ByVal e As DependencyPropertyChangedEventArgs)
            If IsBusy Then
                If DisplayAfter.Equals(TimeSpan.Zero) Then
                    ' Включить видимость сейчас
                    IsContentVisible = True
                Else
                    ' Задать таймер для включения видимости
                    _displayAfterTimer.Interval = DisplayAfter
                    _displayAfterTimer.Start()
                End If
            Else
                ' Больше не отображать
                _displayAfterTimer.Stop()
                IsContentVisible = False
            End If
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' Возвращает или задает значение, указывающее, отображается ли содержимое занятости пользователю.
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
        ''' Указывает свойство зависимости (dependency property) BusyContent.
        ''' </summary>
        Public Shared ReadOnly BusyContentProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContent", _
            GetType(Object), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Возвращает или задает значение, указывающее на шаблон, который будет использоваться для отображения содержимого занятости пользователю.
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
        ''' Указывает свойство зависимости (dependency property) BusyTemplate.
        ''' </summary>
        Public Shared ReadOnly BusyContentTemplateProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContentTemplate", _
            GetType(DataTemplate), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Возвращает или задает значение, определяющее задержку перед отображением содержимого занятости.
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
        ''' Указывает свойство зависимости (dependency property) DisplayAfter.
        ''' </summary>
        Public Shared ReadOnly DisplayAfterProperty As DependencyProperty = DependencyProperty.Register( _
            "DisplayAfter", _
            GetType(TimeSpan), _
            GetType(BusyIndicator), _
            New PropertyMetadata(TimeSpan.FromSeconds(0.1)))

        ''' <summary>
        ''' Возвращает или задает значение, указывающее стиль для наложения.
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
        ''' Указывает свойство зависимости (dependency property) OverlayStyle.
        ''' </summary>
        Public Shared ReadOnly OverlayStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "OverlayStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Возвращает или задает значение, указывающее стиль для индикатора выполнения.
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
        ''' Указывает свойство зависимости (dependency property) ProgressBarStyle.
        ''' </summary>
        Public Shared ReadOnly ProgressBarStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "ProgressBarStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))
    End Class
End Namespace