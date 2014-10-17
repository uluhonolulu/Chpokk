namespace $safeprojectname$.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// Ein Steuerelement für die Bereitstellung eines visuellen Indikators, wenn eine Anwendung ausgelastet ist.
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
        /// Ruft einen Wert ab, der anzeigt, ob BusyContent sichtbar ist, bzw. legt diesen fest.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        /// <summary>
        /// Timer, der zur Verzögerung der anfänglichen Anzeige und zur Vermeidung des "Flackerns" verwendet wird.
        /// </summary>
        private DispatcherTimer _displayAfterTimer;

        /// <summary>
        /// Instanziiert eine neue Instanz des BusyIndicator-Steuerelements.
        /// </summary>
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            _displayAfterTimer = new DispatcherTimer();
            _displayAfterTimer.Tick += new EventHandler(DisplayAfterTimerElapsed);
        }

        /// <summary>
        /// Überschreibt die OnApplyTemplate-Methode.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        /// Handler für DisplayAfterTimer.
        /// </summary>
        /// <param name="sender">Der Ereignissender.</param>
        /// <param name="e">Ereignisargumente.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// Ändert den visuellen Zustand des bzw. der Steuerelemente.
        /// </summary>
        /// <param name="useTransitions">True, wenn Statusübergänge verwendet werden sollen.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// Ruft einen Wert ab, der anzeigt, ob der ausgelastete Indikator angezeigt werden soll, bzw. legt diesen fest.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// Gibt die IsBusy-Abhängigkeitseigenschaft an.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// Handler der geänderten Eigenschaft "IsBusyProperty".
        /// </summary>
        /// <param name="d">BusyIndicator, der seinen Wert "IsBusy" geändert hat.</param>
        /// <param name="e">Ereignisargumente.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// Handler der geänderten Eigenschaft "IsBusyProperty".
        /// </summary>
        /// <param name="e">Ereignisargumente.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Jetzt sichtbar machen
                    IsContentVisible = true;
                }
                else
                {
                    // Timer zum Sichtbarmachen festlegen
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // Nicht mehr sichtbar
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }
            ChangeVisualState(true);
        }

        /// <summary>
        /// Ruft einen Wert ab, der den ausgelasteten Inhalt angibt, der dem Benutzer angezeigt werden soll, bzw. ruft diesen ab.
        /// </summary>
        public object BusyContent
        {
            get { return (object)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        /// Gibt die BusyContent-Abhängigkeitseigenschaft an.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Ruft einen Wert ab, der die Vorlage angibt, mit welcher der ausgelastete Inhalt dem Benutzer angezeigt werden soll, bzw. ruft diesen ab.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        /// Gibt die BusyTemplate-Abhängigkeitseigenschaft an.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Ruft einen Wert ab, der angibt, nach welcher Verzögerung der ausgelastete Inhalt dem Benutzer angezeigt werden soll, bzw. legt diesen fest.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        /// Gibt die DisplayAfter-Abhängigkeitseigenschaft an.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// Ruft einen Wert ab, der den für das Overlay zu verwendenden Stil angibt, bzw. legt diesen fest.
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        /// Gibt die OverlayStyle-Abhängigkeitseigenschaft an.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Ruft einen Wert ab, der den für den Fortschrittsbalken zu verwendenden Stil angibt, bzw. legt diesen fest.
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        /// <summary>
        /// Gibt die ProgressBarStyle-Abhängigkeitseigenschaft an.
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));
    }
}