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
Imports System.Runtime.InteropServices
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.OLE.Interop
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VisualStudio.TextManager.Interop
Imports Microsoft.VsSDK.UnitTestLibrary

Namespace $safeprojectname$.EditorTests
	''' <summary>
	''' Help code to create ILocalRegistry mock
	''' </summary>
	Friend Class LocalRegistryServiceMock
		Private Sub New()
		End Sub
		Friend Shared Function GetILocalRegistryInstance() As BaseMock
			Dim factory As GenericMockFactory = New GenericMockFactory("ILocalRegistry", New Type() { GetType(ILocalRegistry) })
			Dim mockObj As BaseMock = factory.GetInstance()
			Dim name As String = String.Format("{0}.{1}", GetType(ILocalRegistry).FullName, "CreateInstance")
			mockObj.AddMethodCallback(name, New EventHandler(Of CallbackArgs)(AddressOf CreateInstanceCallBack))
			Return mockObj
		End Function

		#Region "Callbacks"
		Private Shared Sub CreateInstanceCallBack(ByVal caller As Object, ByVal arguments As CallbackArgs)
			' Create the output mock object for the frame
			Dim textLines As IVsTextLines = CType(GetIVsTextLinesInstance(), IVsTextLines)
			'''GCHandle handle = GCHandle.Alloc(textLines);
			arguments.SetParameter(4, Marshal.GetComInterfaceForObject(textLines, GetType(IVsTextLines)))
			arguments.ReturnValue = VSConstants.S_OK
		End Sub
		#End Region

		Private Shared Function GetIVsTextLinesInstance() As BaseMock
			Dim factory As GenericMockFactory = New GenericMockFactory("IVsTextLines", New Type() { GetType(IVsTextLines), GetType(IObjectWithSite) })
			Dim mockObj As BaseMock = factory.GetInstance()
			Return mockObj
		End Function
	End Class
End Namespace
