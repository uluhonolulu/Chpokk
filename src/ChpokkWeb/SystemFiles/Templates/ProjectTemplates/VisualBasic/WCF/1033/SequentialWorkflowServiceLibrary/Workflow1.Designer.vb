<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Workflow1

    'NOTE: The following procedure is required by the Workflow Designer
    'It can be modified using the Workflow Designer. 
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerNonUserCode()> _
    <System.CodeDom.Compiler.GeneratedCode("","")> _
    Private Sub InitializeComponent()
        Me.CanModifyActivities = True
        Dim activitybind1 As System.Workflow.ComponentModel.ActivityBind = New System.Workflow.ComponentModel.ActivityBind
        Dim workflowparameterbinding1 As System.Workflow.ComponentModel.WorkflowParameterBinding = New System.Workflow.ComponentModel.WorkflowParameterBinding
        Dim activitybind2 As System.Workflow.ComponentModel.ActivityBind = New System.Workflow.ComponentModel.ActivityBind
        Dim workflowparameterbinding2 As System.Workflow.ComponentModel.WorkflowParameterBinding = New System.Workflow.ComponentModel.WorkflowParameterBinding
        Dim typedoperationinfo1 As System.Workflow.Activities.TypedOperationInfo = New System.Workflow.Activities.TypedOperationInfo
        Dim workflowserviceattributes1 As System.Workflow.Activities.WorkflowServiceAttributes = New System.Workflow.Activities.WorkflowServiceAttributes
        Me.receiveActivity1 = New System.Workflow.Activities.ReceiveActivity
        '
        'receiveActivity1
        '
        Me.receiveActivity1.CanCreateInstance = True
        Me.receiveActivity1.Name = "receiveActivity1"
        activitybind1.Name = "Workflow1"
        activitybind1.Path = "ReturnValue"
        workflowparameterbinding1.ParameterName = "(ReturnValue)"
        workflowparameterbinding1.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, CType(activitybind1, System.Workflow.ComponentModel.ActivityBind))
        activitybind2.Name = "Workflow1"
        activitybind2.Path = "InputValue"
        workflowparameterbinding2.ParameterName = "value"
        workflowparameterbinding2.SetBinding(System.Workflow.ComponentModel.WorkflowParameterBinding.ValueProperty, CType(activitybind2, System.Workflow.ComponentModel.ActivityBind))
        Me.receiveActivity1.ParameterBindings.Add(workflowparameterbinding1)
        Me.receiveActivity1.ParameterBindings.Add(workflowparameterbinding2)
        typedoperationinfo1.ContractType = GetType($safeprojectname$.IWorkflow1)
        typedoperationinfo1.Name = "GetData"
        Me.receiveActivity1.ServiceOperationInfo = typedoperationinfo1
        workflowserviceattributes1.ConfigurationName = "Workflow1"
        workflowserviceattributes1.Name = "Workflow1"
        '
        'Workflow1
        '
        Me.Activities.Add(Me.receiveActivity1)
        Me.Name = "Workflow1"
        Me.SetValue(System.Workflow.Activities.ReceiveActivity.WorkflowServiceAttributesProperty, workflowserviceattributes1)
        Me.CanModifyActivities = False

    End Sub
    Private receiveActivity1 As System.Workflow.Activities.ReceiveActivity

End Class
