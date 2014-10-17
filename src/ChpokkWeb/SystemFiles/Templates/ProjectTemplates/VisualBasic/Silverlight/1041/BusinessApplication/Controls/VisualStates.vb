
Namespace Controls
    ''' <summary>
    ''' コントロールの表示状態の名前およびヘルパーです。
    ''' </summary>
    Friend Module VisualStates

#Region "GroupBusyStatus"
        ''' <summary>
        ''' BusyIndicator の Busy 状態です。
        ''' </summary>
        Public Const StateBusy As String = "Busy"

        ''' <summary>
        ''' BusyIndicator の Idle 状態です。
        ''' </summary>
        Public Const StateIdle As String = "Idle"

        ''' <summary>
        ''' ビジー状態のグループ名です。
        ''' </summary>
        Public Const GroupBusyStatus As String = "BusyStatusStates"
#End Region

#Region "GroupVisibility"
        ''' <summary>
        ''' BusyIndicator の Visible 状態の名前です。
        ''' </summary>
        Public Const StateVisible As String = "Visible"

        ''' <summary>
        ''' BusyIndicator の Hidden 状態の名前です。
        ''' </summary>
        Public Const StateHidden As String = "Hidden"

        ''' <summary>
        ''' BusyDisplay グループです。
        ''' </summary>
        Public Const GroupVisibility As String = "VisibilityStates"
#End Region
    End Module
End Namespace