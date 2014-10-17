
Namespace Controls
    ''' <summary>
    ''' Nomi e supporti per gli stati di visualizzazione nei controlli.
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' Stato Busy per BusyIndicator.
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' Stato Idle per BusyIndicator.
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' Nome gruppo di stati Busy.
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' Nome stato Visible per BusyIndicator.
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' Nome stato Hidden per BusyIndicator.
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' Gruppo BusyDisplay.
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace