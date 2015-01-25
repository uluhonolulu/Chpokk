'------------------------------------------------------------------------------
' $WinForm_VB_MyApplication_Designer_AutoGen$
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On


Namespace My
    
    '$WinForm_VB_MyApplication_Designer_Class$
    '
    Partial Friend Class MyApplication
        
        <Global.System.Diagnostics.DebuggerStepThroughAttribute()>  _
        Public Sub New()
            MyBase.New(Global.Microsoft.VisualBasic.ApplicationServices.AuthenticationMode.Windows)
            Me.IsSingleInstance = false
            Me.EnableVisualStyles = true
            Me.SaveMySettingsOnExit = true
            Me.ShutDownStyle = Global.Microsoft.VisualBasic.ApplicationServices.ShutdownMode.AfterMainFormCloses
        End Sub
        
        <Global.System.Diagnostics.DebuggerStepThroughAttribute()>  _
        Protected Overrides Sub OnCreateMainForm()
            Me.MainForm = Global.$safeprojectname$.Form1
        End Sub
    End Class
End Namespace
