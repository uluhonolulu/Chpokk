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
Imports System.Text
Imports Microsoft.VsSDK.UnitTestLibrary
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.Shell.Interop

Namespace $safeprojectname$.MyToolWindowTest
	Friend Class WindowFrameMock
		Private Const propertiesName As String = "properties"

		Private Shared frameFactory As GenericMockFactory = Nothing

		''' <summary>
		''' Return a IVsWindowFrame without any special implementation
		''' </summary>
		''' <returns></returns>
		Friend Shared Function GetBaseFrame() As IVsWindowFrame
			If frameFactory Is Nothing Then
                frameFactory = New GenericMockFactory("WindowFrame", New Type() {GetType(IVsWindowFrame), GetType(IVsWindowFrame2)})
			End If
			Dim frame As IVsWindowFrame = CType(frameFactory.GetInstance(), IVsWindowFrame)
			Return frame
		End Function
	End Class
End Namespace
