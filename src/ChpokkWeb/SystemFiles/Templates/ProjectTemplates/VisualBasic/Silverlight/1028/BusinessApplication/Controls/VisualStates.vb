
Namespace Controls
    ''' <summary>
    ''' 控制項中可見狀態的名稱和 Helper。
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' BusyIndicator 的 Busy 狀態。
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' BusyIndicator 的 Idle 狀態。
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' 忙碌狀態群組名稱。
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' BusyIndicator 的 Visible 狀態名稱。
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' BusyIndicator 的 Hidden 狀態名稱。
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' BusyDisplay 群組。
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace