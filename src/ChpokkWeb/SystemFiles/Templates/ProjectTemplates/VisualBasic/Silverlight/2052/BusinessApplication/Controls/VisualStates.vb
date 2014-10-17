
Namespace Controls
    ''' <summary>
    ''' 控件中的可视状态的名称和帮助程序。
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' BusyIndicator 的 Busy 状态。
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' BusyIndicator 的 Idle 状态。
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' 繁忙状态组名称。
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' BusyIndicator 的 Visible 状态名称。
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' BusyIndicator 的 Hidden 状态名称。
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' BusyDisplay 组。
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace