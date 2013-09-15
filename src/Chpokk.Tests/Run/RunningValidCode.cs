using System;
using System.Collections.Generic;
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
			var assembly = Assembly.LoadFile(path);
			assembly.EntryPoint.Invoke(null, new object[]{null});
			//
		}
	}
}
