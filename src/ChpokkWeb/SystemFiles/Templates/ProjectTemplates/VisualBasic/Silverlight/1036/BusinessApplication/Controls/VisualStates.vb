
Namespace Controls
    ''' <summary>
    ''' Noms et programmes d'assistance pour les états visuels dans les contrôles.
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' État Busy pour BusyIndicator.
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' État Idle pour BusyIndicator.
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' Nom de groupe d'états Busy.
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' Nom d'état Visible pour BusyIndicator.
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' Nom d'état Hidden pour BusyIndicator.
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' Groupe BusyDisplay.
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace