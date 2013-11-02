using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Reflection;
using System.Text;
using Gallio.Framework;
using ICSharpCode.PackageManagement.Cmdlets;
using ICSharpCode.PackageManagement.Scripting;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace NuNuNuGet {
	[TestFixture]
	public class LetsSee {
		[Test]
		public void Test() {
			//RunScript(
			//	@"Install-Package nunit -solution D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Twitter\Chpokk-SampleSol\src\ChpokkSampleSolution.sln");
			//PowerShell ps = PowerShell.Create();
			//ps.AddCommand("Install-Package");
			var installPackageCmdlet = new InstallPackageCmdlet() { Id = "nunit", Solution = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Twitter\Chpokk-SampleSol\src\ChpokkSampleSolution.sln", ProjectName = "ConsoleApplication1" };
			var methodInfo = typeof (InstallPackageCmdlet).GetMethod("ProcessRecord", BindingFlags.Instance | BindingFlags.NonPublic);
			methodInfo.Invoke(installPackageCmdlet, null);
			//var results = installPackageCmdlet.Invoke();
			//foreach (var result in results) {
			//	System.Console.WriteLine(result);
			//}
		}

		[Test]
		public void MaybeWeCanAtleastRunTheScript() {
			var scriptFileName = new PackageInitializeScriptFileName(@"D:\Projects\Chpokk\src\packages\LibGit2Sharp.0.10");
			new PackageInstallScript(null, scriptFileName).Run(null);
		}

		private string RunScript(string scriptText)
{
    // create Powershell runspace

    Runspace runspace = RunspaceFactory.CreateRunspace();

    // open it

    runspace.Open();

    // create a pipeline and feed it the script text

    Pipeline pipeline = runspace.CreatePipeline();
    pipeline.Commands.AddScript(scriptText);

    // add an extra command to transform the script
    // output objects into nicely formatted strings

    // remove this line to get the actual objects
    // that the script returns. For example, the script

    // "Get-Process" returns a collection
    // of System.Diagnostics.Process instances.

    pipeline.Commands.Add("Out-String");

    // execute the script

    var results = pipeline.Invoke();

    // close the runspace

    runspace.Close();

    // convert the script result into a single string

    StringBuilder stringBuilder = new StringBuilder();
    foreach (PSObject obj in results)
    {
        stringBuilder.AppendLine(obj.ToString());
    }

    return stringBuilder.ToString();
}
	}
}
