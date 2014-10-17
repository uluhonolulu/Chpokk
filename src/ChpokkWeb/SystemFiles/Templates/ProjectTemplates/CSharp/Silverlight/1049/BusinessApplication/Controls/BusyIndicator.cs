namespace $safeprojectname$.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// Элемент управления, отображающий занятость приложения.
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
        /// Возвращает или задает значение, указывающее, отображается ли BusyContent.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        /// <summary>
        /// Таймер (Timer) позволяет отложить первоначальное отображение и избежать мигания.
        /// </summary>
        private DispatcherTimer _displayAfterTimer;

        /// <summary>
        /// Создает экземпляр элемента управления BusyIndicator.
        /// </summary>
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            _displayAfterTimer = new DispatcherTimer();
            _displayAfterTimer.Tick += new EventHandler(DisplayAfterTimerElapsed);
        }

        /// <summary>
        /// Переопределяет метод OnApplyTemplate.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        /// Обработчик события DisplayAfterTimer.
        /// </summary>
        /// <param name="sender">Отправитель события.</param>
        /// <param name="e">Аргументы события.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// Изменяет визуальные состояния элементов управления.
        /// </summary>
        /// <param name="useTransitions">Значение True, если должны использоваться переходы состояния.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее, должен ли отображаться индикатор занятости.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// Указывает свойство зависимости (dependency property) IsBusy.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// Обработчик изменения свойства IsBusyProperty.
        /// </summary>
        /// <param name="d">Объект BusyIndicator, который изменяет свое состояние IsBusy.</param>
        /// <param name="e">Аргументы события.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// Обработчик изменения свойства IsBusyProperty.
        /// </summary>
        /// <param name="e">Аргументы события.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Включить видимость сейчас
                    IsContentVisible = true;
                }
                else
                {
                    // Задать таймер для включения видимости
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // Больше не отображать
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }
            ChangeVisualState(true);
        }

        /// <summary>
        /// Возвращает или задает значение, указывающее, отображается ли содержимое занятости пользователю.
        /// </summary>
        public object BusyContent
        {
            get { return (object)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        /// Указывает свойство зависимости (dependency property) BusyContent.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Возвращает или задает значение, указывающее на шаблон, который будет использоваться для отображения содержимого занятости пользователю.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        /// Указывает свойство зависимости (dependency property) BusyTemplate.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Возвращает или задает значение, определяющее задержку перед отображением содержимого занятости.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        /// Указывает свойство зависимости (dependency property) DisplayAfter.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// Возвращает или задает значение, указывающее стиль для наложения.
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        /// Указывает свойство зависимости (dependency property) OverlayStyle.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Возвращает или задает значение, указывающее стиль для индикатора выполнения.
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        /// <summary>
        /// Указывает свойство зависимости (dependency property) ProgressBarStyle.
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));
    }
}