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

Namespace $safeprojectname$
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
		''' Get an IVsUiShell that implements SetWaitCursor, SaveDocDataToFile, ShowMessageBox
		''' </summary>
		''' <returns>uishell mock</returns>
		Friend Shared Function GetUiShellInstance0() As BaseMock
			Dim uiShell As BaseMock = GetUiShellInstance()
			Dim name As String = String.Format("{0}.{1}", GetType(IVsUIShell).FullName, "SetWaitCursor")
			uiShell.AddMethodCallback(name, New EventHandler(Of CallbackArgs)(AddressOf SetWaitCursorCallBack))

			name = String.Format("{0}.{1}", GetType(IVsUIShell).FullName, "SaveDocDataToFile")
			uiShell.AddMethodCallback(name, New EventHandler(Of CallbackArgs)(AddressOf SaveDocDataToFileCallBack))

			name = String.Format("{0}.{1}", GetType(IVsUIShell).FullName, "ShowMessageBox")
			uiShell.AddMethodCallback(name, New EventHandler(Of CallbackArgs)(AddressOf ShowMessageBoxCallBack))
			Return uiShell
		End Function
		#End Region

		#Region "Callbacks"
		Private Shared Sub SetWaitCursorCallBack(ByVal caller As Object, ByVal arguments As CallbackArgs)
			arguments.ReturnValue = VSConstants.S_OK
		End Sub

		Private Shared Sub SaveDocDataToFileCallBack(ByVal caller As Object, ByVal arguments As CallbackArgs)
			arguments.ReturnValue = VSConstants.S_OK
		End Sub

		Private Shared Sub ShowMessageBoxCallBack(ByVal caller As Object, ByVal arguments As CallbackArgs)
			arguments.ReturnValue = VSConstants.S_OK
			arguments.SetParameter(10, CInt(Fix(System.Windows.Forms.DialogResult.Yes)))
		End Sub

		#End Region
	End Class
End Namespace