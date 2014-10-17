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
Imports System.Collections.Generic
Imports System.ComponentModel.Design
Imports System.Globalization
Imports System.Text
Imports System.Runtime.InteropServices
Imports System.Threading
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VisualStudio.Shell
Imports Microsoft.VSSDK.Tools.VsIdeTesting
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports EnvDTE
Imports $packageNamespace$

Namespace $safeprojectname$
    ''' <summary>
    ''' Summary description for ToolwindowTest
    ''' </summary>
    <TestClass()> _
    Public Class ToolWindowTest

        Private Delegate Sub ThreadInvoker()

        <TestMethod(), HostType("VS IDE")> _
        Public Sub ShowToolWindow()
            Dim objThreadInvoker As New ThreadInvoker(AddressOf AnonymousMethod1)
            UIThreadInvoker.Invoke(objThreadInvoker)
        End Sub

        ''' <summary>
        ''' </summary>
        Private Sub AnonymousMethod1()

            Dim toolWindowCmd As CommandID = New CommandID(GuidList.$packageCmdSetGuid$, PkgCmdIDList.$cmdidToolWin$)

            ExecuteCommand(toolWindowCmd)

            Dim persistanceGuid As Guid = New Guid(GuidList.guidToolWindowPersistanceString)
            Assert.IsTrue(CanFindToolwindow(persistanceGuid))

            'hide toolwindow
            Dim toolwindow As IVsWindowFrame = GetToolwindow(persistanceGuid, __VSFINDTOOLWIN.FTW_fFindFirst)
            If toolwindow IsNot Nothing Then
                toolwindow.Hide()
            End If

        End Sub


        Private Sub ExecuteCommand(ByVal cmd As CommandID)
            Dim Customin As Object = Nothing
            Dim Customout As Object = Nothing
            Dim guidString As String = cmd.Guid.ToString("B").ToUpper()
            Dim cmdId As Integer = cmd.ID
            Dim dte As EnvDTE.DTE = VsIdeTestHostContext.Dte
            dte.Commands.Raise(guidString, cmdId, Customin, Customout)
        End Sub

        Private Function CanFindToolwindow(ByVal persistenceGuid As Guid) As Boolean

            ' Get the uishell service
            Dim sp As IServiceProvider = VsIdeTestHostContext.ServiceProvider
            Dim uiShellService As IVsUIShell = TryCast(sp.GetService(GetType(SVsUIShell)), IVsUIShell)
            Assert.IsNotNull(uiShellService)

            ' Try find the toolwindow using the uishell service
            Dim windowFrame As IVsWindowFrame = Nothing
            Dim findToolwindowFlags As UInteger = CType(__VSFINDTOOLWIN.FTW_fFindFirst, UInteger)
            Assert.IsTrue(VSConstants.S_OK = uiShellService.FindToolWindow(findToolwindowFlags, persistenceGuid, windowFrame))

            Return (windowFrame IsNot Nothing)
        End Function

        Private Function GetToolwindow(ByVal persistenceGuid As Guid, ByVal findFlags As __VSFINDTOOLWIN) As IVsWindowFrame

            ' Get the uishell service
            Dim sp As IServiceProvider = VsIdeTestHostContext.ServiceProvider
            Dim uiShellService As IVsUIShell = TryCast(sp.GetService(GetType(SVsUIShell)), IVsUIShell)
            Assert.IsNotNull(uiShellService)

            ' Get the toolwindow using the uishell service
            Dim windowFrame As IVsWindowFrame = Nothing
            Dim findToolwindowFlags As UInteger = CType(findFlags, UInteger)
            Assert.IsTrue(VSConstants.S_OK = uiShellService.FindToolWindow(findToolwindowFlags, persistenceGuid, windowFrame))

            Return windowFrame
        End Function
    End Class
End Namespace
