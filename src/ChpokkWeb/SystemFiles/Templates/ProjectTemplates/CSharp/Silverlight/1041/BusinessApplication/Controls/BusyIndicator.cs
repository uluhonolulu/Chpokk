namespace $safeprojectname$.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// アプリケーションがビジー状態の場合に視覚的なインジケーターを提供するコントロールです。
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
        /// BusyContent を表示するかどうかを示す値を取得または設定します。
        /// </summary>
        protected bool IsContentVisible { get; set; }

        /// <summary>
        /// 最初の表示を遅らせ、ちらつきが発生しないようにするために使用される Timer です。
        /// </summary>
        private DispatcherTimer _displayAfterTimer;

        /// <summary>
        /// BusyIndicator コントロールの新しいインスタンスをインスタンス化します。
        /// </summary>
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            _displayAfterTimer = new DispatcherTimer();
            _displayAfterTimer.Tick += new EventHandler(DisplayAfterTimerElapsed);
        }

        /// <summary>
        /// OnApplyTemplate メソッドをオーバーライドします。
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        /// DisplayAfterTimer のハンドラーです。
        /// </summary>
        /// <param name="sender">イベントの送信者です。</param>
        /// <param name="e">イベントの引数です。</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// コントロールの表示状態を変更します。
        /// </summary>
        /// <param name="useTransitions">状態遷移を使用する必要がある場合は True です。</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// ビジー状態のインジケーターを表示するかどうかを示す値を取得または設定します。
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// IsBusy 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// IsBusyProperty プロパティによりハンドラーが変更されました。
        /// </summary>
        /// <param name="d">IsBusy を変更した BusyIndicator です。</param>
        /// <param name="e">イベントの引数です。</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// IsBusyProperty プロパティによりハンドラーが変更されました。
        /// </summary>
        /// <param name="e">イベントの引数です。</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // 今すぐ表示
                    IsContentVisible = true;
                }
                else
                {
                    // タイマーが表示されるように設定
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // もう表示されません
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }
            ChangeVisualState(true);
        }

        /// <summary>
        /// ビジー状態のコンテンツをユーザーに表示することを示す値を取得または設定します。
        /// </summary>
        public object BusyContent
        {
            get { return (object)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        /// BusyContent 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// ビジー状態のコンテンツをユーザーに表示するために使用するテンプレートを示す値を取得または設定します。
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        /// BusyTemplate 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// ビジー状態のコンテンツを表示する前に遅延する時間を示す値を取得または設定します。
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        /// DisplayAfter 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// オーバーレイに使用するスタイルを示す値を取得または設定します。
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        /// OverlayStyle 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// プログレス バーに使用するスタイルを示す値を取得または設定します。
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        /// <summary>
        /// ProgressBarStyle 依存関係プロパティを識別します。
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));
    }
}