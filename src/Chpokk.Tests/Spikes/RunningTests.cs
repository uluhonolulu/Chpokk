using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class RunningTests {
		[Test]
		public void Test() {
			//Must use Gallio.Runner.TestLauncher
			//Look at EchoProgram.RunTests(ILogger logger)
			//
			//gallio.models.helpers.simpletestdriver.runassembly
			//Gallio.dll!Gallio.Framework.Pattern.PatternTestInstanceState.InvokeFixtureMethod(Gallio.Common.Reflection.IMethodInfo method = {Gallio.Common.Reflection.Impl.NativeMethodWrapper}, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePai...

		}
	}
}
