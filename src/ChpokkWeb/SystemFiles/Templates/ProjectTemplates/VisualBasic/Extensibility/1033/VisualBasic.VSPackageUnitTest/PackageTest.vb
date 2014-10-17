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
Imports Microsoft.VsSDK.UnitTestLibrary
Imports Microsoft.VisualStudio.Shell.Interop
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports $packageNamespace$

Namespace $safeprojectname$
	<TestClass()> _
	Public Class PackageTest
		<TestMethod()> _
		Public Sub CreateInstance()
			Dim package As $packageClass$Package = New $packageClass$Package()
		End Sub

		<TestMethod()> _
		Public Sub IsIVsPackage()
			Dim package As $packageClass$Package = New $packageClass$Package()
			Assert.IsNotNull(TryCast(package, IVsPackage), "The object does not implement IVsPackage")
		End Sub

		<TestMethod()> _
		Public Sub SetSite()
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

			' Unsite the package
			Assert.AreEqual(0, package.SetSite(Nothing), "SetSite(null) did not return S_OK")
		End Sub
	End Class
End Namespace
