namespace $safeprojectname$.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// 用于在应用程序繁忙时提供可视指示器的控件。
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
        /// 获取或设置一个值，该值指示 BusyContent 是否可见。
        /// </summary>
        protected bool IsContentVisible { get; set; }

        /// <summary>
        /// 用于延迟初始显示并避免闪烁的 Timer。
        /// </summary>
        private DispatcherTimer _displayAfterTimer;

        /// <summary>
        /// 实例化 BusyIndicator 控件的新实例。
        /// </summary>
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            _displayAfterTimer = new DispatcherTimer();
            _displayAfterTimer.Tick += new EventHandler(DisplayAfterTimerElapsed);
        }

        /// <summary>
        /// 重写 OnApplyTemplate 方法。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        /// DisplayAfterTimer 的处理程序。
        /// </summary>
        /// <param name="sender">事件发送方。</param>
        /// <param name="e">事件参数。</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// 更改控件的可视状态。
        /// </summary>
        /// <param name="useTransitions">如果应使用状态转换，则为 True。</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否应显示繁忙指示器。
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// 标识 IsBusy 依赖属性。
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// IsBusyProperty 属性更改处理程序。
        /// </summary>
        /// <param name="d">更改了其 IsBusy 的 BusyIndicator。</param>
        /// <param name="e">事件参数。</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// IsBusyProperty 属性更改处理程序。
        /// </summary>
        /// <param name="e">事件参数。</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // 立即转为可见
                    IsContentVisible = true;
                }
                else
                {
                    // 设置定时器以转为可见
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // 不再可见
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }
            ChangeVisualState(true);
        }

        /// <summary>
        /// 获取或设置一个值，该值指示要向用户显示的繁忙内容。
        /// </summary>
        public object BusyContent
        {
            get { return (object)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        /// 标识 BusyContent 依赖属性。
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// 获取或设置一个值，该值指示要用于向用户显示繁忙内容的模板。
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        /// 标识 BusyTemplate 依赖属性。
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// 获取或设置一个值，该值指示在显示繁忙内容之前的延迟时间。
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        /// 标识 DisplayAfter 依赖属性。
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// 获取或设置一个值，该值指示要用于覆盖的样式。
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        /// 标识 OverlayStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// 获取或设置一个值，该值指示要用于进度栏的样式。
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        /// <summary>
        /// 标识 ProgressBarStyle 依赖属性。
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));
    }
}