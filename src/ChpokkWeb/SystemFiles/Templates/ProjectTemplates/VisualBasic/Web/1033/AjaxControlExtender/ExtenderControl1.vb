<TargetControlType(GetType(Control))> _
Public Class ExtenderControl1
    Inherits ExtenderControl

    Protected Overrides Function GetScriptDescriptors(ByVal targetControl As System.Web.UI.Control) As IEnumerable(Of ScriptDescriptor)
        Dim descriptor As New ScriptBehaviorDescriptor("$safeprojectname$.ClientBehavior1", targetControl.ClientID)

        Return New List(Of ScriptDescriptor) From {descriptor}
    End Function

    ' Generate the script reference
    Protected Overrides Function GetScriptReferences() As IEnumerable(Of ScriptReference)
        Dim scriptRef As New ScriptReference("$safeprojectname$.ClientBehavior1.js", Me.GetType().Assembly.FullName)

        Return New List(Of ScriptReference) From {scriptRef}
    End Function
End Class
