using System.Collections.Generic;
using System.IO;
using Arractas;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using LibGit2Sharp.Tests.TestHelpers;
using Shouldly;

namespace Chpokk.Tests.Run {
	[TestFixture]
	public class RunningValidCode : RunningCodeBase<CompiledExeWithEmptyCodeContext> {
		[Test]
		public void CanRunTheMainMethod() {
			this.DoesNotThrow.ShouldBe(true);
		}

		[Test, DependsOn("CanRunTheMainMethod")]
		public void TheAssemblyIsNotLocked() {
			Assert.DoesNotThrow(() => File.Delete(Context.ExePath));
		}

		[Test, DependsOn("CanRunTheMainMethod")]
		public void TheResultShouldBeNull() {
			Result.Result.ShouldBe(null);
		}

	}

	public class CompiledExeWithEmptyCodeContext : CompiledExeContext {
		public override string Code {
			get { return "class program {static void Main(){}}"; }
		}
	}
}
