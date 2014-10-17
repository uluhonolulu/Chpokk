
Namespace Controls
    ''' <summary>
    ''' Nombres y aplicaciones auxiliares para los estados visuales de los controles.
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' Estado Busy de BusyIndicator.
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' Estado Idle de BusyIndicator.
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' Nombre de grupo de estados de ocupado.
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' Nombre de estado Visible de BusyIndicator.
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' Nombre de estado Hidden de BusyIndicator.
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' Grupo BusyDisplay.
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace