using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningCodeWithOutput : RunningCodeBase<CompiledExeThatWritesToConsole> {
		[Test]
		public void ResultShouldContainConsoleOutput() {
			Result.StandardOutput.ShouldBe(Context.CONSOLE_OUTPUT);
		}
	}

	public class CompiledExeThatWritesToConsole : CompiledExeContext {
		public override string Code {
			get { return "class program {static void Main(){System.Console.Write(\"message\");}}"; }
		}

		public string CONSOLE_OUTPUT = "message";
	}
}
