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
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VsSDK.UnitTestLibrary

Namespace $safeprojectname$.EditorTests
	Friend Class RegisterEditorsServiceMock
		Private Shared registerEditorFactory As GenericMockFactory

		''' <summary>
		''' Returns an SVsRegisterEditors service that does not implement any methods
		''' </summary>
		''' <returns></returns>
		Private Sub New()
		End Sub
		Friend Shared Function GetRegisterEditorsInstance() As BaseMock
			If registerEditorFactory Is Nothing Then
				registerEditorFactory = New GenericMockFactory("SVsRegisterEditors", New Type() { GetType(IVsRegisterEditors) })
			End If
			Dim registerEditor As BaseMock = registerEditorFactory.GetInstance()
			Return registerEditor
		End Function
	End Class
End Namespace
