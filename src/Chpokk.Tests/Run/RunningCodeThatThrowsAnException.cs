using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningCodeThatThrowsAnException : RunningCodeBase<CompiledExeThatThrowsAnExceptionContext> {
		[Test]
		public void ResultShouldContainErrorMessage() {
			Result.ErrorOutput.ShouldContain("System.Exception: message");
		}
	}

	public class CompiledExeThatThrowsAnExceptionContext : CompiledExeContext {
		public override string Code {
			get { return "class program {static void Main(){ throw new System.Exception(\"message\");}}"; }
		}
	}
}
