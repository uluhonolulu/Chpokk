Namespace My

    '$WinForm_VB_MyEvents_Class$

    Class MyApplication

#If _MyType = "WindowsForms" Then
        '$WinForm_VB_MyEvents_OnInitialize$
        <Global.System.Diagnostics.DebuggerStepThrough()> _
        Protected Overrides Function OnInitialize(ByVal commandLineArgs As System.Collections.ObjectModel.ReadOnlyCollection(Of String)) As Boolean
            'Set the splash screen timeout. 
            Me.MinimumSplashScreenDisplayTime = 2000
            Return MyBase.OnInitialize(commandLineArgs)
        End Function
#End If

    End Class

End Namespace
