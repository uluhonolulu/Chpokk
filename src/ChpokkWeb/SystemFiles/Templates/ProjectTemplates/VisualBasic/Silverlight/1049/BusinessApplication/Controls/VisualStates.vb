
Namespace Controls
    ''' <summary>
    ''' Имена и помощники для визуальных состояний в элементах управления.
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' Состояние занятости (Busy) для элемента BusyIndicator.
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' Неактивное состояние (Idle) для элемента BusyIndicator.
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' Имя группы состояний занятости.
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' Имя состояния видимости Visible для элемента BusyIndicator.
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' Имя состояния невидимости (Hidden) для элемента BusyIndicator.
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' Группа BusyDisplay.
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace