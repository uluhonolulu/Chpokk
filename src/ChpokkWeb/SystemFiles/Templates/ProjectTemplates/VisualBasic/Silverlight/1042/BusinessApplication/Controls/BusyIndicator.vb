Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Shapes
Imports System.Windows.Threading

Namespace Controls
    ''' <summary>
    ''' 응용 프로그램이 사용 중일 때 시각적 표시기를 제공하는 컨트롤입니다.
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
        ''' BusyContent를 표시할지 여부를 나타내는 값을 가져오거나 설정합니다.
        ''' </summary>
        Protected Property IsContentVisible() As Boolean

        ''' <summary>
        ''' 초기 표시를 지연하고 깜박임을 방지하는 데 사용되는 Timer입니다.
        ''' </summary>
        Private _displayAfterTimer As DispatcherTimer

        ''' <summary>
        ''' BusyIndicator 컨트롤의 새 인스턴스를 인스턴스화합니다.
        ''' </summary>
        Public Sub New()
            DefaultStyleKey = GetType(BusyIndicator)
            _displayAfterTimer = New DispatcherTimer()
            AddHandler _displayAfterTimer.Tick, AddressOf DisplayAfterTimerElapsed
        End Sub

        ''' <summary>
        ''' OnApplyTemplate 메서드를 재정의합니다.
        ''' </summary>
        Public Overloads Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            ChangeVisualState(False)
        End Sub

        ''' <summary>
        ''' DisplayAfterTimer에 대한 처리기입니다.
        ''' </summary>
        ''' <param name="sender">이벤트 전송자입니다.</param>
        ''' <param name="e">이벤트 인수입니다.</param>
        Private Sub DisplayAfterTimerElapsed(ByVal sender As Object, ByVal e As EventArgs)
            _displayAfterTimer.Stop()
            IsContentVisible = True
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' 컨트롤의 표시 상태를 변경합니다.
        ''' </summary>
        ''' <param name="useTransitions">상태 전환을 사용해야 하는 경우 True입니다.</param>
        Protected Overridable Sub ChangeVisualState(ByVal useTransitions As Boolean)
            VisualStateManager.GoToState(Me, If(IsBusy, VisualStates.StateBusy, VisualStates.StateIdle), useTransitions)
            VisualStateManager.GoToState(Me, If(IsContentVisible, VisualStates.StateVisible, VisualStates.StateHidden), useTransitions)
        End Sub

        ''' <summary>
        ''' 사용 중 표시기를 표시할지 여부를 나타내는 값을 가져오거나 설정합니다.
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
        ''' IsBusy 종속성 속성을 식별합니다.
        ''' </summary>
        Public Shared ReadOnly IsBusyProperty As DependencyProperty = DependencyProperty.Register( _
            "IsBusy", _
            GetType(Boolean), _
            GetType(BusyIndicator), _
            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsBusyChanged)))

        ''' <summary>
        ''' IsBusyProperty 속성 변경 처리기입니다.
        ''' </summary>
        ''' <param name="d">IsBusy를 변경한 BusyIndicator입니다.</param>
        ''' <param name="e">이벤트 인수입니다.</param>
        Private Shared Sub OnIsBusyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            DirectCast(d, BusyIndicator).OnIsBusyChanged(e)
        End Sub

        ''' <summary>
        ''' IsBusyProperty 속성 변경 처리기입니다.
        ''' </summary>
        ''' <param name="e">Event 인수입니다.</param>
        Protected Overridable Sub OnIsBusyChanged(ByVal e As DependencyPropertyChangedEventArgs)
            If IsBusy Then
                If DisplayAfter.Equals(TimeSpan.Zero) Then
                    ' 지금 표시
                    IsContentVisible = True
                Else
                    ' 타이머를 표시로 설정
                    _displayAfterTimer.Interval = DisplayAfter
                    _displayAfterTimer.Start()
                End If
            Else
                ' 더 이상 표시되지 않음
                _displayAfterTimer.Stop()
                IsContentVisible = False
            End If
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' 사용자에게 표시할 사용 중 콘텐츠를 나타내는 값을 가져오거나 설정합니다.
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
        ''' BusyContent 종속성 속성을 식별합니다.
        ''' </summary>
        Public Shared ReadOnly BusyContentProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContent", _
            GetType(Object), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' 사용자에게 사용 중 콘텐츠를 표시하는 데 사용할 템플릿을 나타내는 값을 가져오거나 설정합니다.
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
        ''' BusyTemplate 종속성 속성을 식별합니다.
        ''' </summary>
        Public Shared ReadOnly BusyContentTemplateProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContentTemplate", _
            GetType(DataTemplate), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' 사용 중 콘텐츠를 표시하기 이전의 지연 시간을 나타내는 값을 가져오거나 설정합니다.
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
        ''' DisplayAfter 종속성 속성을 식별합니다.
        ''' </summary>
        Public Shared ReadOnly DisplayAfterProperty As DependencyProperty = DependencyProperty.Register( _
            "DisplayAfter", _
            GetType(TimeSpan), _
            GetType(BusyIndicator), _
            New PropertyMetadata(TimeSpan.FromSeconds(0.1)))

        ''' <summary>
        ''' 오버레이에 대해 사용할 스타일을 나타내는 값을 가져오거나 설정합니다.
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
        ''' OverlayStyle 종속성 속성을 식별합니다.
        ''' </summary>
        Public Shared ReadOnly OverlayStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "OverlayStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' 진행률 표시줄에 사용할 스타일을 나타내는 값을 가져오거나 설정합니다.
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
        ''' ProgressBarStyle 종속성 속성을 식별합니다.
        ''' </summary>
        Public Shared ReadOnly ProgressBarStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "ProgressBarStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))
    End Class
End Namespace