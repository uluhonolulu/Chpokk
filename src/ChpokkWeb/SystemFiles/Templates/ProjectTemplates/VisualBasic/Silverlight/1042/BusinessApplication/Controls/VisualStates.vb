
Namespace Controls
    ''' <summary>
    ''' 컨트롤의 표시 상태에 대한 이름 및 도우미입니다.
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' BusyIndicator에 대한 Busy 상태입니다.
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' BusyIndicator에 대한 Idle 상태입니다.
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' 사용 중 상태 그룹 이름입니다.
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' BusyIndicator에 대한 Visible 상태 이름입니다.
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' BusyIndicator에 대한 Hidden 상태 이름입니다.
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' BusyDisplay 그룹입니다.
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace