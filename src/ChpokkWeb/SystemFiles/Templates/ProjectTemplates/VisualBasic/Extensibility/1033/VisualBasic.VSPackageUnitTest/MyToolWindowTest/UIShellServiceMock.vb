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
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VsSDK.UnitTestLibrary

Namespace $safeprojectname$.MyToolWindowTest
	Friend Class UIShellServiceMock
		Private Shared uiShellFactory As GenericMockFactory

		#Region "UiShell Getters"
		''' <summary>
		''' Returns an IVsUiShell that does not implement any methods
		''' </summary>
		''' <returns></returns>
		Private Sub New()
		End Sub
		Friend Shared Function GetUiShellInstance() As BaseMock
			If uiShellFactory Is Nothing Then
				uiShellFactory = New GenericMockFactory("UiShell", New Type() { GetType(IVsUIShell), GetType(IVsUIShellOpenDocument) })
			End If
			Dim uiShell As BaseMock = uiShellFactory.GetInstance()
			Return uiShell
		End Function

		''' <summary>
		''' Get an IVsUiShell that implement CreateToolWindow
		''' </summary>
		''' <returns>uishell mock</returns>
		Friend Shared Function GetUiShellInstanceCreateToolWin() As BaseMock
			Dim uiShell As BaseMock = GetUiShellInstance()
			Dim name As String = String.Format("{0}.{1}", GetType(IVsUIShell).FullName, "CreateToolWindow")
			uiShell.AddMethodCallback(name, New EventHandler(Of CallbackArgs)(AddressOf CreateToolWindowCallBack))

			Return uiShell
		End Function

		''' <summary>
		''' Get an IVsUiShell that implement CreateToolWindow (negative test)
		''' </summary>
		''' <returns>uishell mock</returns>
		Friend Shared Function GetUiShellInstanceCreateToolWinReturnsNull() As BaseMock
			Dim uiShell As BaseMock = GetUiShellInstance()
			Dim name As String = String.Format("{0}.{1}", GetType(IVsUIShell).FullName, "CreateToolWindow")
			uiShell.AddMethodCallback(name, New EventHandler(Of CallbackArgs)(AddressOf CreateToolWindowNegativeTestCallBack))

			Return uiShell
		End Function
		#End Region

		#Region "Callbacks"
		Private Shared Sub CreateToolWindowCallBack(ByVal caller As Object, ByVal arguments As CallbackArgs)
			arguments.ReturnValue = VSConstants.S_OK

			' Create the output mock object for the frame
			Dim frame As IVsWindowFrame = WindowFrameMock.GetBaseFrame()
			arguments.SetParameter(9, frame)
		End Sub

		Private Shared Sub CreateToolWindowNegativeTestCallBack(ByVal caller As Object, ByVal arguments As CallbackArgs)
			arguments.ReturnValue = VSConstants.S_OK

			'set the windowframe object to null
			arguments.SetParameter(9, Nothing)
		End Sub
		#End Region
	End Class
End Namespace