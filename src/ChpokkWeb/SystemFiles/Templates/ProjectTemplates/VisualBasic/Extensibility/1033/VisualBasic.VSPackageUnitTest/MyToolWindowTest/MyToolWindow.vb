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
    ''' <summary>
    '''This is a test class for MyToolWindowTest and is intended
    '''to contain all MyToolWindowTest Unit Tests
    '''</summary>
    <TestClass()> _
    Public Class MyToolWindowTest

        ''' <summary>
        '''MyToolWindow Constructor test
        '''</summary>
        <TestMethod()> _
        Public Sub ValidateToolWindowShown()
            Dim target As MyToolWindow = New MyToolWindow()
            Assert.IsNotNull(target, "Failed to create an instance of MyToolWindow")

            Dim method As MethodInfo = target.GetType().GetMethod("get_Content", BindingFlags.Public Or BindingFlags.Instance)
            Assert.IsNotNull(method.Invoke(target, Nothing), "MyControl object was not instantiated")
        End Sub

        ''' <summary>
        '''Verify the Content property is valid.
        '''</summary>
        <TestMethod()> _
        Public Sub WindowPropertyTest()
            Dim target As MyToolWindow = New MyToolWindow()
            Assert.IsNotNull(target.Content, "Content property was null")
        End Sub

    End Class
End Namespace
