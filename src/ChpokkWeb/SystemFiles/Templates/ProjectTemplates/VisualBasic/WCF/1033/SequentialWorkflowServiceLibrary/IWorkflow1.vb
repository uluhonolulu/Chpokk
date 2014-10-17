' NOTE: You can use the "Rename" command on the context menu to change the interface name "IWorkflow1" in both code and config file together.
<ServiceContract()> _
Public Interface IWorkflow1

    <OperationContract()> _
    Function GetData(ByVal value As Integer) As String

    ' TODO: Add your service operations here

End Interface
