Imports System
Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Shapes
Imports System.Windows.Threading

Namespace Controls
    ''' <summary>
    ''' Controllo per fornire un indicatore visivo quando un'applicazione è occupata.
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
        ''' Ottiene o imposta un valore che indica se l'oggetto BusyContent è visibile.
        ''' </summary>
        Protected Property IsContentVisible() As Boolean

        ''' <summary>
        ''' Timer utilizzato per ritardare la visualizzazione iniziale ed evitare lo sfarfallio.
        ''' </summary>
        Private _displayAfterTimer As DispatcherTimer

        ''' <summary>
        ''' Crea una nuova istanza del controllo BusyIndicator.
        ''' </summary>
        Public Sub New()
            DefaultStyleKey = GetType(BusyIndicator)
            _displayAfterTimer = New DispatcherTimer()
            AddHandler _displayAfterTimer.Tick, AddressOf DisplayAfterTimerElapsed
        End Sub

        ''' <summary>
        ''' Esegue l'override del metodo OnApplyTemplate.
        ''' </summary>
        Public Overloads Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()
            ChangeVisualState(False)
        End Sub

        ''' <summary>
        ''' Gestore per DisplayAfterTimer.
        ''' </summary>
        ''' <param name="sender">Mittente dell'evento.</param>
        ''' <param name="e">Argomenti dell'evento.</param>
        Private Sub DisplayAfterTimerElapsed(ByVal sender As Object, ByVal e As EventArgs)
            _displayAfterTimer.Stop()
            IsContentVisible = True
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' Modifica lo stato o gli stati di visualizzazione del controllo.
        ''' </summary>
        ''' <param name="useTransitions">True se devono essere utilizzate le transizioni di stato.</param>
        Protected Overridable Sub ChangeVisualState(ByVal useTransitions As Boolean)
            VisualStateManager.GoToState(Me, If(IsBusy, VisualStates.StateBusy, VisualStates.StateIdle), useTransitions)
            VisualStateManager.GoToState(Me, If(IsContentVisible, VisualStates.StateVisible, VisualStates.StateHidden), useTransitions)
        End Sub

        ''' <summary>
        ''' Ottiene o imposta un valore che indica se visualizzare l'indicatore di occupato.
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
        ''' Identifica la proprietà di dipendenza IsBusy.
        ''' </summary>
        Public Shared ReadOnly IsBusyProperty As DependencyProperty = DependencyProperty.Register( _
            "IsBusy", _
            GetType(Boolean), _
            GetType(BusyIndicator), _
            New PropertyMetadata(False, New PropertyChangedCallback(AddressOf OnIsBusyChanged)))

        ''' <summary>
        ''' Gestore modificato dalla proprietà IsBusyProperty.
        ''' </summary>
        ''' <param name="d">Oggetto BusyIndicator che ha modificato il relativo oggetto IsBusy.</param>
        ''' <param name="e">Argomenti dell'evento.</param>
        Private Shared Sub OnIsBusyChanged(ByVal d As DependencyObject, ByVal e As DependencyPropertyChangedEventArgs)
            DirectCast(d, BusyIndicator).OnIsBusyChanged(e)
        End Sub

        ''' <summary>
        ''' Gestore modificato dalla proprietà IsBusyProperty.
        ''' </summary>
        ''' <param name="e">Argomenti di Event.</param>
        Protected Overridable Sub OnIsBusyChanged(ByVal e As DependencyPropertyChangedEventArgs)
            If IsBusy Then
                If DisplayAfter.Equals(TimeSpan.Zero) Then
                    ' Diventare visibile ora
                    IsContentVisible = True
                Else
                    ' Impostare un timer per diventare visibile
                    _displayAfterTimer.Interval = DisplayAfter
                    _displayAfterTimer.Start()
                End If
            Else
                ' Non essere più visibile
                _displayAfterTimer.Stop()
                IsContentVisible = False
            End If
            ChangeVisualState(True)
        End Sub

        ''' <summary>
        ''' Ottiene o imposta un valore che indica il contenuto dello stato di occupato da visualizzare all'utente.
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
        ''' Identifica la proprietà di dipendenza BusyContent.
        ''' </summary>
        Public Shared ReadOnly BusyContentProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContent", _
            GetType(Object), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Ottiene o imposta un valore che indica il modello da utilizzare per la visualizzazione del contenuto dello stato di occupato.
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
        ''' Identifica la proprietà di dipendenza BusyTemplate.
        ''' </summary>
        Public Shared ReadOnly BusyContentTemplateProperty As DependencyProperty = DependencyProperty.Register( _
            "BusyContentTemplate", _
            GetType(DataTemplate), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Ottiene o imposta un valore che indica il tempo di ritardo prima della visualizzazione del contenuto dello stato di occupato.
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
        ''' Identifica la proprietà di dipendenza DisplayAfter.
        ''' </summary>
        Public Shared ReadOnly DisplayAfterProperty As DependencyProperty = DependencyProperty.Register( _
            "DisplayAfter", _
            GetType(TimeSpan), _
            GetType(BusyIndicator), _
            New PropertyMetadata(TimeSpan.FromSeconds(0.1)))

        ''' <summary>
        ''' Ottiene o imposta un valore che indica lo stile da utilizzare per la sovrapposizione.
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
        ''' Identifica la proprietà di dipendenza OverlayStyle.
        ''' </summary>
        Public Shared ReadOnly OverlayStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "OverlayStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))

        ''' <summary>
        ''' Ottiene o imposta un valore che indica lo stile da utilizzare per l'indicatore di stato.
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
        ''' Identifica la proprietà di dipendenza ProgressBarStyle.
        ''' </summary>
        Public Shared ReadOnly ProgressBarStyleProperty As DependencyProperty = DependencyProperty.Register( _
            "ProgressBarStyle", _
            GetType(Style), _
            GetType(BusyIndicator), _
            New PropertyMetadata(Nothing))
    End Class
End Namespace