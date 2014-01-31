using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Arractas;
using ChpokkWeb.Features.Running;
using MbUnit.Framework;
using LibGit2Sharp.Tests.TestHelpers;
using Shouldly;

namespace Chpokk.Tests.Run {
	public class RunningCodeBase<TContext> : BaseQueryTest<TContext, ExeRunnerOutput> where TContext : CompiledExeContext, new() {


		public override ExeRunnerOutput Act() {
			var exePath = Context.ExePath;
			File.Exists(exePath).ShouldBe(true);
			DoesNotThrow = true;
			try {
				var consoleOutput = string.Empty;
				var errorOutput = string.Empty;
				return new ExeRunnerOutput{Result = Context.Container.Get<ExeRunner>().RunMain(exePath, c => consoleOutput += c, c => errorOutput += c), StandardOutput = consoleOutput, ErrorOutput = errorOutput}; ;
			}
			catch (Exception exception) {
				Console.WriteLine(exception);
				DoesNotThrow = false;
				return null;
			}
		}

		public bool DoesNotThrow { get; set; }
	}

	public class ExeRunnerOutput {
		public object Result { get; set; }

		public string StandardOutput { get; set; }

		public string ErrorOutput { get; set; }
	}
}
