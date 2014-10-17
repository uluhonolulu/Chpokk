Public Class ServerControl1
    Inherits ScriptControl

    Protected Overrides Function GetScriptDescriptors() As IEnumerable(Of ScriptDescriptor)
        Dim descriptor As New ScriptControlDescriptor("$safeprojectname$.ClientControl1", Me.ClientID)

        Return New List(Of ScriptDescriptor) From {descriptor}
    End Function

    ' Generate the script reference
    Protected Overrides Function GetScriptReferences() As IEnumerable(Of ScriptReference)
        Dim scriptRef As New ScriptReference("$safeprojectname$.ClientControl1.js", Me.GetType().Assembly.FullName)

        Return New List(Of ScriptReference) From {scriptRef}
    End Function
End Class
