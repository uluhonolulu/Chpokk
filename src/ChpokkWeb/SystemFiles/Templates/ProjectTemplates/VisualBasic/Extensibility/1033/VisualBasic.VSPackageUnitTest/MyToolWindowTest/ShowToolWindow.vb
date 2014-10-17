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
Imports Microsoft.VSSDK.Tools.VsIdeTesting
Imports $packageNamespace$

Namespace $safeprojectname$.MyToolWindowTest
    <TestClass()> _
    Public Class ShowToolWindowTest

        <TestMethod()> _
        Public Sub ValidateToolWindowShown()
            Dim package As IVsPackage = TryCast(New $packageClass$Package(), IVsPackage)

            ' Create a basic service provider
            Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()

            'Add uishell service that knows how to create a toolwindow
            Dim uiShellService As BaseMock = UIShellServiceMock.GetUiShellInstanceCreateToolWin()
            serviceProvider.AddService(GetType(SVsUIShell), uiShellService, False)
            $if$($hasEditor$==True)
            ' Add site support to register editor factory
            Dim registerEditor As BaseMock = $safeprojectname$.EditorTests.RegisterEditorsServiceMock.GetRegisterEditorsInstance()
            serviceProvider.AddService(GetType(SVsRegisterEditors), registerEditor, False)
            $endif$
            ' Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK")

            Dim method As MethodInfo = GetType($packageClass$Package).GetMethod("ShowToolWindow", BindingFlags.NonPublic Or BindingFlags.Instance)

            Dim result As Object = method.Invoke(package, New Object() {Nothing, Nothing})
        End Sub

        <TestMethod(), ExpectedException(GetType(InvalidOperationException), "Did not throw expected exception when windowframe object was null")> _
        Public Sub ShowToolwindowNegativeTest()
            Dim package As IVsPackage = TryCast(New $packageClass$Package(), IVsPackage)

            ' Create a basic service provider
            Dim serviceProvider As OleServiceProvider = OleServiceProvider.CreateOleServiceProviderWithBasicServices()

            'Add uishell service that knows how to create a toolwindow
            Dim uiShellService As BaseMock = UIShellServiceMock.GetUiShellInstanceCreateToolWinReturnsNull()
            serviceProvider.AddService(GetType(SVsUIShell), uiShellService, False)
            $if$($hasEditor$==True)
            ' Add site support to register editor factory
            Dim registerEditor As BaseMock = $safeprojectname$.EditorTests.RegisterEditorsServiceMock.GetRegisterEditorsInstance()
            serviceProvider.AddService(GetType(SVsRegisterEditors), registerEditor, False)
            $endif$
            ' Site the package
            Assert.AreEqual(0, package.SetSite(serviceProvider), "SetSite did not return S_OK")

            Dim method As MethodInfo = GetType($packageClass$Package).GetMethod("ShowToolWindow", BindingFlags.NonPublic Or BindingFlags.Instance)

            'Invoke thows TargetInvocationException, but we want it's inner Exception thrown by ShowToolWindow, InvalidOperationException.
            Try
                Dim result As Object = method.Invoke(package, New Object() {Nothing, Nothing})
            Catch e As Exception
                Throw e.InnerException
            End Try
        End Sub
    End Class
End Namespace
