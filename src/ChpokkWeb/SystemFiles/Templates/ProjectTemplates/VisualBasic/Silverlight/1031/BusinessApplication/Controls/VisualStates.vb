
Namespace Controls
    ''' <summary>
    ''' Namen und Helfer für visuelle Zustände in den Steuerelementen.
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' Der Zustand "Busy" für BusyIndicator.
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' Der Zustand "Idle" für BusyIndicator.
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' Gruppenname der Zustände "Busy".
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' Name des Zustands "Visible" für BusyIndicator.
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' Name des Zustands "Hidden" für BusyIndicator.
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' Gruppe "BusyDisplay"
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace