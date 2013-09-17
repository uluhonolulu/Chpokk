using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using LibGit2Sharp.Tests.TestHelpers;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningCodeWithArguments: RunningCodeBase<CompiledExeWithMainMethodWithArgumentsContext> {
		[Test]
		public void CanRunTheMainMethod() {
			this.DoesNotThrow.ShouldBe(true);
		}
	}

	public class CompiledExeWithMainMethodWithArgumentsContext : CompiledExeContext {
		public override string Code {
			get { return "class program {static void Main(string[] args){}}"; }
		}
	}
}
