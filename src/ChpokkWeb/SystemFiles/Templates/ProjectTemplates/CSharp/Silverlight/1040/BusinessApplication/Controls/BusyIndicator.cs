namespace $safeprojectname$.Controls
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Shapes;
    using System.Windows.Threading;
    
    /// <summary>
    /// Controllo per fornire un indicatore visivo quando un'applicazione è occupata.
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
        /// Ottiene o imposta un valore che indica se l'oggetto BusyContent è visibile.
        /// </summary>
        protected bool IsContentVisible { get; set; }

        /// <summary>
        /// Timer utilizzato per ritardare la visualizzazione iniziale ed evitare lo sfarfallio.
        /// </summary>
        private DispatcherTimer _displayAfterTimer;

        /// <summary>
        /// Crea una nuova istanza del controllo BusyIndicator.
        /// </summary>
        public BusyIndicator()
        {
            DefaultStyleKey = typeof(BusyIndicator);
            _displayAfterTimer = new DispatcherTimer();
            _displayAfterTimer.Tick += new EventHandler(DisplayAfterTimerElapsed);
        }

        /// <summary>
        /// Esegue l'override del metodo OnApplyTemplate.
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            ChangeVisualState(false);
        }

        /// <summary>
        /// Gestore per DisplayAfterTimer.
        /// </summary>
        /// <param name="sender">Mittente dell'evento.</param>
        /// <param name="e">Argomenti dell'evento.</param>
        private void DisplayAfterTimerElapsed(object sender, EventArgs e)
        {
            _displayAfterTimer.Stop();
            IsContentVisible = true;
            ChangeVisualState(true);
        }

        /// <summary>
        /// Modifica lo stato o gli stati di visualizzazione del controllo.
        /// </summary>
        /// <param name="useTransitions">True se devono essere utilizzate le transizioni di stato.</param>
        protected virtual void ChangeVisualState(bool useTransitions)
        {
            VisualStateManager.GoToState(this, IsBusy ? VisualStates.StateBusy : VisualStates.StateIdle, useTransitions);
            VisualStateManager.GoToState(this, IsContentVisible ? VisualStates.StateVisible : VisualStates.StateHidden, useTransitions);
        }

        /// <summary>
        /// Ottiene o imposta un valore che indica se visualizzare l'indicatore di occupato.
        /// </summary>
        public bool IsBusy
        {
            get { return (bool)GetValue(IsBusyProperty); }
            set { SetValue(IsBusyProperty, value); }
        }

        /// <summary>
        /// Identifica la proprietà di dipendenza IsBusy.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = DependencyProperty.Register(
            "IsBusy",
            typeof(bool),
            typeof(BusyIndicator),
            new PropertyMetadata(false, new PropertyChangedCallback(OnIsBusyChanged)));

        /// <summary>
        /// Gestore modificato dalla proprietà IsBusyProperty.
        /// </summary>
        /// <param name="d">Oggetto BusyIndicator che ha modificato il relativo oggetto IsBusy.</param>
        /// <param name="e">Argomenti dell'evento.</param>
        private static void OnIsBusyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((BusyIndicator)d).OnIsBusyChanged(e);
        }

        /// <summary>
        /// Gestore modificato dalla proprietà IsBusyProperty.
        /// </summary>
        /// <param name="e">Argomenti dell'evento.</param>
        protected virtual void OnIsBusyChanged(DependencyPropertyChangedEventArgs e)
        {
            if (IsBusy)
            {
                if (DisplayAfter.Equals(TimeSpan.Zero))
                {
                    // Diventare visibile ora
                    IsContentVisible = true;
                }
                else
                {
                    // Impostare un timer per diventare visibile
                    _displayAfterTimer.Interval = DisplayAfter;
                    _displayAfterTimer.Start();
                }
            }
            else
            {
                // Non essere più visibile
                _displayAfterTimer.Stop();
                IsContentVisible = false;
            }
            ChangeVisualState(true);
        }

        /// <summary>
        /// Ottiene o imposta un valore che indica il contenuto dello stato di occupato da visualizzare all'utente.
        /// </summary>
        public object BusyContent
        {
            get { return (object)GetValue(BusyContentProperty); }
            set { SetValue(BusyContentProperty, value); }
        }

        /// <summary>
        /// Identifica la proprietà di dipendenza BusyContent.
        /// </summary>
        public static readonly DependencyProperty BusyContentProperty = DependencyProperty.Register(
            "BusyContent",
            typeof(object),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Ottiene o imposta un valore che indica il modello da utilizzare per la visualizzazione del contenuto dello stato di occupato.
        /// </summary>
        public DataTemplate BusyContentTemplate
        {
            get { return (DataTemplate)GetValue(BusyContentTemplateProperty); }
            set { SetValue(BusyContentTemplateProperty, value); }
        }

        /// <summary>
        /// Identifica la proprietà di dipendenza BusyTemplate.
        /// </summary>
        public static readonly DependencyProperty BusyContentTemplateProperty = DependencyProperty.Register(
            "BusyContentTemplate",
            typeof(DataTemplate),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Ottiene o imposta un valore che indica il tempo di ritardo prima della visualizzazione del contenuto dello stato di occupato.
        /// </summary>
        public TimeSpan DisplayAfter
        {
            get { return (TimeSpan)GetValue(DisplayAfterProperty); }
            set { SetValue(DisplayAfterProperty, value); }
        }

        /// <summary>
        /// Identifica la proprietà di dipendenza DisplayAfter.
        /// </summary>
        public static readonly DependencyProperty DisplayAfterProperty = DependencyProperty.Register(
            "DisplayAfter",
            typeof(TimeSpan),
            typeof(BusyIndicator),
            new PropertyMetadata(TimeSpan.FromSeconds(0.1)));

        /// <summary>
        /// Ottiene o imposta un valore che indica lo stile da utilizzare per la sovrapposizione.
        /// </summary>
        public Style OverlayStyle
        {
            get { return (Style)GetValue(OverlayStyleProperty); }
            set { SetValue(OverlayStyleProperty, value); }
        }

        /// <summary>
        /// Identifica la proprietà di dipendenza OverlayStyle.
        /// </summary>
        public static readonly DependencyProperty OverlayStyleProperty = DependencyProperty.Register(
            "OverlayStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));

        /// <summary>
        /// Ottiene o imposta un valore che indica lo stile da utilizzare per l'indicatore di stato.
        /// </summary>
        public Style ProgressBarStyle
        {
            get { return (Style)GetValue(ProgressBarStyleProperty); }
            set { SetValue(ProgressBarStyleProperty, value); }
        }

        /// <summary>
        /// Identifica la proprietà di dipendenza ProgressBarStyle.
        /// </summary>
        public static readonly DependencyProperty ProgressBarStyleProperty = DependencyProperty.Register(
            "ProgressBarStyle",
            typeof(Style),
            typeof(BusyIndicator),
            new PropertyMetadata(null));
    }
}