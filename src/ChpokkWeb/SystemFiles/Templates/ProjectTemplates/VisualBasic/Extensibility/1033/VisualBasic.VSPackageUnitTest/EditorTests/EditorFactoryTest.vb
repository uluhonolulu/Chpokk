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
Imports System.Reflection
Imports Microsoft.VisualStudio
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Microsoft.VsSDK.UnitTestLibrary
Imports $packageNamespace$

Namespace $safeprojectname$.EditorTests
	<TestClass()> _
	Public Class EditorFactoryTest
		<TestMethod()> _
		Public Sub CreateInstance()
			Dim package As $packageClass$Package = New $packageClass$Package()

			Dim editorFactory As EditorFactory = New EditorFactory(package)
			Assert.IsNotNull(editorFactory, "Failed to initialize new instance of EditorFactory.")
		End Sub

		<TestMethod()> _
		Public Sub IsIVsEditorFactory()
			Dim package As $packageClass$Package = New $packageClass$Package()
			Dim editorFactory As EditorFactory = New EditorFactory(package)
			Assert.IsNotNull(TryCast(editorFactory, IVsEditorFactory), "The object does not implement IVsEditorFactory")
		End Sub

		<TestMethod()> _
		Public Sub SetSite()
			Dim package As $packageClass$Package = New $packageClass$Package()

			'Create the editor factory
			Dim editorFactory As EditorFactory = New EditorFactory(package)

			' Create a basic service provider
			Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()

			' Site the editor factory
			Assert.AreEqual(0, editorFactory.SetSite(serviceProvider), "SetSite did not return S_OK")
		End Sub

		<TestMethod()> _
		Public Sub CreateEditorInstance()
			Dim package As $packageClass$Package = New $packageClass$Package()

			'Create the editor factory
			Dim editorFactory As EditorFactory = New EditorFactory(package)

			' Create a basic service provider
			Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()
			serviceProvider.AddService(GetType(SLocalRegistry), CType(LocalRegistryServiceMock.GetILocalRegistryInstance(), ILocalRegistry), True)

			' Site the editor factory
			Assert.AreEqual(0, editorFactory.SetSite(serviceProvider), "SetSite did not return S_OK")

			Dim ppunkDocView As IntPtr
			Dim ppunkDocData As IntPtr
			Dim pbstrEditorCaption As String = String.Empty
			Dim pguidCmdUI As Guid = Guid.Empty
			Dim pgrfCDW As Integer = 0
			Assert.AreEqual(VSConstants.S_OK, editorFactory.CreateEditorInstance(VSConstants.CEF_OPENFILE, Nothing, Nothing, Nothing, 0, IntPtr.Zero, ppunkDocView, ppunkDocData, pbstrEditorCaption, pguidCmdUI, pgrfCDW))
		End Sub

		<TestMethod()> _
		Public Sub CheckLogicalView()
			Dim package As $packageClass$Package = New $packageClass$Package()

			'Create the editor factory
			Dim editorFactory As EditorFactory = New EditorFactory(package)

			' Create a basic service provider
			Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()
			serviceProvider.AddService(GetType(SLocalRegistry), CType(LocalRegistryServiceMock.GetILocalRegistryInstance(), ILocalRegistry), True)

			' Site the editor factory
			Assert.AreEqual(0, editorFactory.SetSite(serviceProvider), "SetSite did not return S_OK")

			Dim ppunkDocView As IntPtr
			Dim ppunkDocData As IntPtr
			Dim pbstrEditorCaption As String = String.Empty
			Dim pguidCmdUI As Guid = Guid.Empty
			Dim pgrfCDW As Integer = 0
			Assert.AreEqual(VSConstants.S_OK, editorFactory.CreateEditorInstance(VSConstants.CEF_OPENFILE, Nothing, Nothing, Nothing, 0, IntPtr.Zero, ppunkDocView, ppunkDocData, pbstrEditorCaption, pguidCmdUI, pgrfCDW)) 'check for successfull creation of editor instance

			Dim bstrPhysicalView As String = String.Empty
			Dim refGuidLogicalView As Guid = VSConstants.LOGVIEWID_Debugging
			Assert.AreEqual(VSConstants.E_NOTIMPL, editorFactory.MapLogicalView(refGuidLogicalView, bstrPhysicalView))

			refGuidLogicalView = VSConstants.LOGVIEWID_Code
			Assert.AreEqual(VSConstants.E_NOTIMPL, editorFactory.MapLogicalView(refGuidLogicalView, bstrPhysicalView))

			refGuidLogicalView = VSConstants.LOGVIEWID_TextView
			Assert.AreEqual(VSConstants.S_OK, editorFactory.MapLogicalView(refGuidLogicalView, bstrPhysicalView))

			refGuidLogicalView = VSConstants.LOGVIEWID_UserChooseView
			Assert.AreEqual(VSConstants.E_NOTIMPL, editorFactory.MapLogicalView(refGuidLogicalView, bstrPhysicalView))

			refGuidLogicalView = VSConstants.LOGVIEWID_Primary
			Assert.AreEqual(VSConstants.S_OK, editorFactory.MapLogicalView(refGuidLogicalView, bstrPhysicalView))
		End Sub

		''' <summary>
		''' Verify that the object implements the IDisposable interface
		''' </summary>
		<TestMethod()> _
		Public Sub IsIDisposableTest()
			Dim package As $packageClass$Package = New $packageClass$Package()

			Using editorFactory As EditorFactory = New EditorFactory(package)
				Assert.IsNotNull(TryCast(editorFactory, IDisposable), "The object does not implement IDisposable interface")
			End Using
		End Sub

		''' <summary>
		''' Object is destroyed deterministically by Dispose() method call test
		''' </summary>
		<TestMethod()> _
		Public Sub DisposeTest()
			Dim package As $packageClass$Package = New $packageClass$Package()

			Dim editorFactory As EditorFactory = New EditorFactory(package)
			editorFactory.Dispose()
		End Sub

		''' <summary>
		''' Check that all disposable members are disposed after Dispose method call
		''' </summary>
		<TestMethod()> _
		Public Sub DisposeDisposableMembersTest()
			Dim package As $packageClass$Package = New $packageClass$Package()

			Dim editorFactory As EditorFactory = New EditorFactory(package)
			Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()
			editorFactory.SetSite(serviceProvider)
			Dim service As Object = editorFactory.GetService(GetType(IProfferService))
			Assert.IsNotNull(service)
			editorFactory.Dispose() 'service provider contains no services after this call
			service = editorFactory.GetService(GetType(IProfferService))
			Assert.IsNull(service, "serviceprovider has not beed disposed as expected")
		End Sub

		''' <summary>
		'''A test for Close ()
		'''</summary>
		<TestMethod()> _
		Public Sub CloseTest()
			Dim package As $packageClass$Package = New $packageClass$Package()

			Dim editorFactory As EditorFactory = New EditorFactory(package)
			Assert.AreEqual(VSConstants.S_OK, editorFactory.Close(), "Close did no return S_OK")
		End Sub
	End Class
End Namespace