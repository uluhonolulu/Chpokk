using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Reflection;
using Gallio.Framework;
using ICSharpCode.PackageManagement.Cmdlets;
using ICSharpCode.PackageManagement.Design;
using ICSharpCode.PackageManagement.Scripting;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using NuGet;
using PackageManagement.Cmdlets.Tests;
using PackageManagement.Cmdlets.Tests.Helpers;

namespace NuNuNuGet {
	[TestFixture]
	public class LetsSee : CmdletTestsBase {
		[Test]
		public void Test() {
			CreateCmdletWithActivePackageSourceAndProject();

			SetIdParameter("Test");
			RunCmdlet();

			string actualPackageId = fakeProject.LastInstallPackageCreated.PackageId;
			Assert.AreEqual("Test", actualPackageId);
		
		}
		
		TestableInstallPackageCmdlet cmdlet;
		FakeCmdletTerminatingError fakeTerminatingError;
		FakePackageManagementProject fakeProject;
		
		void CreateCmdletWithoutActiveProject()
		{
			cmdlet = new TestableInstallPackageCmdlet();
			fakeTerminatingError = cmdlet.FakeCmdletTerminatingError;
			fakeConsoleHost = cmdlet.FakePackageManagementConsoleHost;
			fakeProject = fakeConsoleHost.FakeProject;
		}
		
		void CreateCmdletWithActivePackageSourceAndProject()
		{
			CreateCmdletWithoutActiveProject();
			AddPackageSourceToConsoleHost();
			AddDefaultProjectToConsoleHost();
		}

		void RunCmdlet()
		{
			cmdlet.CallProcessRecord();
		}
		
		void SetIdParameter(string id)
		{
			cmdlet.Id = id;
		}
		
		void EnableIgnoreDependenciesParameter()
		{
			cmdlet.IgnoreDependencies = new SwitchParameter(true);
		}
		
		void EnablePrereleaseParameter()
		{
			cmdlet.IncludePrerelease = new SwitchParameter(true);
		}
		
		void SetSourceParameter(string source)
		{
			cmdlet.Source = source;
		}
		
		void SetVersionParameter(SemanticVersion version)
		{
			cmdlet.Version = version;
		}
		
		void SetProjectNameParameter(string name)
		{
			cmdlet.ProjectName = name;
		}
	}
}
