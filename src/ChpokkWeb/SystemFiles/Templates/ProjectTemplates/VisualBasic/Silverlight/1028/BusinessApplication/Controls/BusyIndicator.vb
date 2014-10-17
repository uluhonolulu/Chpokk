Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Shapes
Imports System.Windows.Threading

Namespace Controls
    ''' <summary>
    ''' 於應用程式忙碌時提供視覺化指標的控制項。
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
        ''' 取得或設定表示 BusyContent 是否可見的值。
        ''' </summary>
        Protected Property IsContentVisible() As Boolean

        ''' <summary>
        ''' 用來延遲初始顯示和避免閃爍的 Timer。
        ''' </summary>
        Private _displayAfterTimer As DispatcherTimer

        ''' <summary>
        ''' 具現化 BusyIndicator 控制項的新執行個體。
        ''' </summary>
        Public Sub New()
            DefaultStyleKey = GetType(BusyIndicator)
            _displayAfterTimer = New DispatcherTimer()
            AddHandler _displayAfterTimer.Tick, AddressOf DisplayAfterTimerElapsed
        End Sub

        ''' <summary>
        ''' 覆寫 OnApplyTemplate 方法。
        ''' </summary>
        Public Overloads Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            ChangeVisualState(False)
        End Sub

        ''' <summary>
        ''' DisplayAfterTimer 的處理常式。
        ''' </summary>
        ''' <param name="sender">事件發送者。</param>
        ''' <param name="e">事件引數。</param>
        Private Sub DisplayAfterTimerElapsed(ByVal sender As Object, ByVal e As EventArgs)
            _displayAfterTimer.Stop()
            IsContentVisible = True
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' 變更控制項的可見狀態。
        ''' </summary>
        ''' <param name="useTransitions">如果應使用狀態轉換，則為 True。</param>
        Protected Overridable Sub ChangeVisualState(ByVal useTransitions As Boolean)
            VisualStateManager.GoToState(Me, If(IsBusy, VisualStates.StateBusy, VisualStates.StateIdle), useTransitions)
            VisualStateManager.GoToState(Me, If(IsContentVisible, VisualStates.StateVisible, VisualStates.StateHidden), useTransitions)
        End Sub

        ''' <summary>
        ''' 取得或設定表示是否要顯示忙碌指標的值。
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
        ''' 識別 IsBusy 相依性屬性。
        ''' </summary>
        Public Shared ReadOnly IsBusyProperty As DependencyProperty = DependencyProperty.Register( _
            "IsBusy", _
            GetType(Boolean), _
            GetType(BusyIndicator), _
            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsBusyChanged)))

        ''' <summary>
        ''' IsBusyProperty 屬性已變更處理常式。
        ''' </summary>
        ''' <param name="d">變更其 IsBusy 的 BusyIndicator。</param>
        ''' <param name="e">事件引數。</param>
        Private Shared Sub OnIsBusyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            DirectCast(d, BusyIndicator).OnIsBusyChanged(e)
        End Sub

        ''' <summary>
        ''' IsBusyProperty 屬性已變更處理常式。
        ''' </summary>
        ''' <param name="e">Event 引數。</param>
        Protected Overridable Sub OnIsBusyChanged(ByVal e As DependencyPropertyChangedEventArgs)
            If IsBusy Then
                If DisplayAfter.Equals(TimeSpan.Zero) Then
                    ' 立即成為可見
                    IsContentVisible = True
                Else
                    ' 設定計時器以便成為可見
                    _displayAfterTimer.Interval = DisplayAfter
                    _displayAfterTimer.Start()
                End If
            Else
                ' 不再是可見
                _displayAfterTimer.Stop()
                IsContentVisible = False
            End If
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' 取得或設定表示要向使用者顯示之忙碌內容的值。
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
        ''' 識別 BusyContent 相依性屬性。
        ''' </summary>
        Public Shared ReadOnly BusyContentProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContent", _
            GetType(Object), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' 取得或設定表示要用來向使用者顯示忙碌內容之範本的值。
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
        ''' 識別 BusyTemplate 相依性屬性。
        ''' </summary>
        Public Shared ReadOnly BusyContentTemplateProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContentTemplate", _
            GetType(DataTemplate), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' 取得或設定表示在顯示忙碌內容前要延遲多久的值。
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
        ''' 識別 DisplayAfter 相依性屬性。
        ''' </summary>
        Public Shared ReadOnly DisplayAfterProperty As DependencyProperty = DependencyProperty.Register( _
            "DisplayAfter", _
            GetType(TimeSpan), _
            GetType(BusyIndicator), _
            New PropertyMetadata(TimeSpan.FromSeconds(0.1)))

        ''' <summary>
        ''' 取得或設定表示要使用於重疊之樣式的值。
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
        ''' 識別 OverlayStyle 相依性屬性。
        ''' </summary>
        Public Shared ReadOnly OverlayStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "OverlayStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' 取得或設定表示要使用於進度列之樣式的值。
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
        ''' 識別 ProgressBarStyle 相依性屬性。
        ''' </summary>
        Public Shared ReadOnly ProgressBarStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "ProgressBarStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))
    End Class
End Namespace