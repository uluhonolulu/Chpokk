using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningCodeAndReturningAValue : RunningCodeBase<CompiledExeWithMainMethodReturningValueContext> {
		[Test]
		public void TheResultShouldContainTheReturnedValue() {
			Result.Result.ShouldBe(Context.RETURN_VALUE);
		}
	}

	public class CompiledExeWithMainMethodReturningValueContext : CompiledExeContext {
		public override string Code {
			get { return "class program {static int Main(){return 5;}}"; }
		}

		public int RETURN_VALUE = 5;

	}
}
