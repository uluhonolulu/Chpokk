using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningValidCode {
		[Test]
		public void Test() {
			var path =
				@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\__anonymous__\Chpokk-SampleSol\src\ConsoleApplication1\bin\Debug\ConsoleApplication1.exe";
			var appDomain = AppDomain.CreateDomain("runner");
			var loader = (AssemblyLoader) appDomain.CreateInstanceFromAndUnwrap(typeof(AssemblyLoader).Assembly.CodeBase,
			                                                                               typeof (AssemblyLoader).FullName, null);
			Assert.DoesNotExist(AppDomain.CurrentDomain.GetAssemblies(), assembly => assembly.FullName.Contains("ConsoleApplication1"));
			loader.Run(path);
			AppDomain.Unload(appDomain);
		}
	}

	public class AssemblyLoader: MarshalByRefObject {
	    public void Run(string exePath) {
		    var assembly = Assembly.LoadFrom(exePath);
			assembly.EntryPoint.Invoke(null, new object[]{null});
	    }
	}
}
