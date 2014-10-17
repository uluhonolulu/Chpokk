namespace $safeprojectname$.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// 응용 프로그램이 사용 중일 때 시각적 표시기를 제공하는 컨트롤입니다.
    /// </summary>
    [TemplateVisualState(Name = VisualStates.StateIdle, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateBusy, GroupName = VisualStates.GroupBusyStatus)]
    [TemplateVisualState(Name = VisualStates.StateVisible, GroupName = VisualStates.GroupVisibility)]
    [TemplateVisualState(Name = VisualStates.StateHidden, GroupName = VisualStates.GroupVisibility)]
    [StyleTypedProperty(Property = "OverlayStyle", StyleTargetType = typeof(Rectangle))]
    [StyleTypedProperty(Property = "ProgressBarStyle", StyleTargetType = typeof(ProgressBar))]
    public class BusyIndicator : ContentControl
    {
        /// <summary>
        /// BusyContent를 표시할지 여부를 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        /// <summary>
        /// 초기 표시를 지연하고 깜박임을 방지하는 데 사용되는 Timer입니다.
        /// </summary>
        private DispatcherTimer _displayAfterTimer;

        /// <summary>
        /// BusyIndicator 컨트롤의 새 인스턴스를 인스턴스화합니다.
        /// </summary>
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            _displayAfterTimer = new DispatcherTimer();
            _displayAfterTimer.Tick += new EventHandler(DisplayAfterTimerElapsed);
        }

        /// <summary>
        /// OnApplyTemplate 메서드를 재정의합니다.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        /// DisplayAfterTimer에 대한 처리기입니다.
        /// </summary>
        /// <param name="sender">이벤트 전송자입니다.</param>
        /// <param name="e">이벤트 인수입니다.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// 컨트롤의 표시 상태를 변경합니다.
        /// </summary>
        /// <param name="useTransitions">상태 전환을 사용해야 하는 경우 True입니다.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// 사용 중 표시기를 표시할지 여부를 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// IsBusy 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// IsBusyProperty 속성 변경 처리기입니다.
        /// </summary>
        /// <param name="d">IsBusy를 변경한 BusyIndicator입니다.</param>
        /// <param name="e">이벤트 인수입니다.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// IsBusyProperty 속성 변경 처리기입니다.
        /// </summary>
        /// <param name="e">이벤트 인수입니다.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // 지금 표시
                    IsContentVisible = true;
                }
                else
                {
                    // 타이머를 표시로 설정
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // 더 이상 표시되지 않음
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }
            ChangeVisualState(true);
        }

        /// <summary>
        /// 사용자에게 표시할 사용 중 콘텐츠를 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        public object BusyContent
        {
            get { return (object)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        /// BusyContent 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// 사용자에게 사용 중 콘텐츠를 표시하는 데 사용할 템플릿을 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        /// BusyTemplate 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// 사용 중 콘텐츠를 표시하기 이전의 지연 시간을 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        /// DisplayAfter 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// 오버레이에 대해 사용할 스타일을 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        /// OverlayStyle 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// 진행률 표시줄에 사용할 스타일을 나타내는 값을 가져오거나 설정합니다.
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        /// <summary>
        /// ProgressBarStyle 종속성 속성을 식별합니다.
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));
    }
}