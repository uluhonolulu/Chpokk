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
Imports System.Collections
Imports System.Text
Imports System.Reflection
Imports System.ComponentModel.Design
Imports Microsoft.VsSDK.UnitTestLibrary
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Microsoft.VisualStudio.Shell
Imports $packageNamespace$

Namespace $safeprojectname$.MenuItemTests
	<TestClass()> _
	Public Class MenuItemTest
		''' <summary>
		''' Verify that a new menu command object gets added to the OleMenuCommandService. 
		''' This action takes place In the Initialize method of the Package object
		''' </summary>
		<TestMethod> _
		Public Sub InitializeMenuCommand()
			' Create the package
            Dim package As IVsPackage = TryCast(New $packageClass$Package(), IVsPackage)
			Assert.IsNotNull(package, "The object does not implement IVsPackage")

			' Create a basic service provider
			Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()
            $if$($hasEditor$==True)
            ' Add site support to register editor factory
			Dim registerEditor As BaseMock = $safeprojectname$.EditorTests.RegisterEditorsServiceMock.GetRegisterEditorsInstance()
			serviceProvider.AddService(GetType(SVsRegisterEditors), registerEditor, False)
            $endif$
            ' Site the package
			Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK")

			'Verify that the menu command can be found
			Dim menuCommandID As CommandID = New CommandID($packageNamespace$.GuidList.$packageCmdSetGuid$, CInt(Fix($packageNamespace$.PkgCmdIDList.$cmdidMenuItem$)))
			Dim info As System.Reflection.MethodInfo = GetType(Package).GetMethod("GetService", BindingFlags.Instance Or BindingFlags.NonPublic)
			Assert.IsNotNull(info)
			Dim mcs As OleMenuCommandService = TryCast(info.Invoke(package, New Object() { (GetType(IMenuCommandService)) }), OleMenuCommandService)
			Assert.IsNotNull(mcs.FindCommand(menuCommandID))
		End Sub

		<TestMethod> _
		Public Sub MenuItemCallback()
			' Create the package
            Dim package As IVsPackage = TryCast(New $packageClass$Package(), IVsPackage)
			Assert.IsNotNull(package, "The object does not implement IVsPackage")

			' Create a basic service provider
			Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()

			' Create a UIShell service mock and proffer the service so that it can called from the MenuItemCallback method
			Dim uishellMock As BaseMock = UIShellServiceMock.GetUiShellInstance()
			serviceProvider.AddService(GetType(SVsUIShell), uishellMock, True)
            $if$($hasEditor$==True)
            ' Add site support to register editor factory
			Dim registerEditor As BaseMock = $safeprojectname$.EditorTests.RegisterEditorsServiceMock.GetRegisterEditorsInstance()
			serviceProvider.AddService(GetType(SVsRegisterEditors), registerEditor, False)
            $endif$
            ' Site the package
			Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK")

			'Invoke private method on package class and observe that the method does not throw
			Dim info As System.Reflection.MethodInfo = CType(package, Object).GetType().GetMethod("MenuItemCallback", BindingFlags.Instance Or BindingFlags.NonPublic)
			Assert.IsNotNull(info, "Failed to get the private method MenuItemCallback throug refplection")
			info.Invoke(package, New Object() { Nothing, Nothing })

			'Clean up services
			serviceProvider.RemoveService(GetType(SVsUIShell))

		End Sub
	End Class
End Namespace
