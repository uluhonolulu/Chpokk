'**************************************************************************

'Copyright (c) Microsoft Corporation. All rights reserved.
'This code is licensed under the Visual Studio SDK license terms.
'THIS CODE IS PROVIDED *AS IS* WITHOUT WARRANTY OF
'ANY KIND, EITHER EXPRESS OR IMPLIED, INCLUDING ANY
'IMPLIED WARRANTIES OF FITNESS FOR A PARTICULAR
'PURPOSE, MERCHANTABILITY, OR NON-INFRINGEMENT.

'**************************************************************************

Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel.Design
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Microsoft.VisualStudio.OLE.Interop
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VisualStudio.Shell
Imports Microsoft.VSSDK.Tools.VsIdeTesting

Namespace $safeprojectname$

    <TestClass()> _
    Public Class EditorTest
        Private Delegate Sub ThreadInvoker()

        Private testContextInstance As TestContext

        ''' <summary>
        '''Gets or sets the test context which provides
        '''information about and functionality for the current test run.
        '''</summary>
        Public Property TestContext() As TestContext
            Get
                Return testContextInstance
            End Get
            Set(ByVal value As TestContext)
                testContextInstance = value
            End Set
        End Property

        ''' <summary>
        '''A test for opening the editor
        '''</summary>
        <TestMethod(), HostType("VS IDE")> _
        Public Sub ValidateNewFileOpenedWithEditor()
            UIThreadInvoker.Invoke(CType(AddressOf AnonymousMethod1, ThreadInvoker))
        End Sub

        Private Sub AnonymousMethod1()

            Dim testUtils As Microsoft.VsSDK.IntegrationTestLibrary.TestUtils = New Microsoft.VsSDK.IntegrationTestLibrary.TestUtils()
            testUtils.CloseCurrentSolution(__VSSLNSAVEOPTIONS.SLNSAVEOPT_NoSave)
            testUtils.CreateEmptySolution(TestContext.TestDir, "CreateEmptySolution")

            'Add new file to the solution and save all
            Dim name As String = "mynewfile"
            Dim dte As EnvDTE.DTE = VsIdeTestHostContext.Dte
            Dim win As EnvDTE.Window = dte.ItemOperations.NewFile("$EditorName$ Files\$EditorName$", name, EnvDTE.Constants.vsViewKindPrimary)
            Assert.IsNotNull(win)
            dte.ExecuteCommand("File.SaveAll", String.Empty)

            'get the currect misc files state
            Dim OriginalValueMiscFilesSavesLastNItems As Object = dte.Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value
            If CInt(Fix(OriginalValueMiscFilesSavesLastNItems)) = 0 Then
                dte.Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value = 5
            End If

            'get a handle to the project item in the solution explorer
            Dim item As EnvDTE.ProjectItem = win.Document.ProjectItem
            Assert.IsNotNull(item)

            'close window
            win.Close(EnvDTE.vsSaveChanges.vsSaveChangesNo)

            'reset the miscfiles property if it was modified
            If Not OriginalValueMiscFilesSavesLastNItems Is dte.Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value Then
                dte.Properties("Environment", "Documents").Item("MiscFilesProjectSavesLastNItems").Value = OriginalValueMiscFilesSavesLastNItems
            End If
        End Sub

    End Class
End Namespace
