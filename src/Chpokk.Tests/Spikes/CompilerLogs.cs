using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Compilation;
using ChpokkWeb.Features.Compilation;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Framework;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class CompilerLogs : BaseCommandTest<BuildableProjectWithSingleRootFileContext>, ILogger {
		[Test]
		public void Test() {
			//
			// TODO: Add test logic here
			//
		}

		public override void Act() {
			var compiler = Context.Container.Get<MsBuildCompiler>();
			compiler.Compile(Context.ProjectPath, this);
		}

		public void Initialize(IEventSource eventSource) {
			this.Verbosity = LoggerVerbosity.Quiet;
			//eventSource.AnyEventRaised += (sender, args) => {
			//	if (!Ignore(args)) {
			//		Console.WriteLine();
			//		Console.WriteLine(args.Message);
			//		Console.WriteLine(args.GetType());
			//		foreach (var propertyInfo in args.GetType().GetProperties()) {
			//			if (propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string)) {
			//				Console.WriteLine(propertyInfo.Name + ": " + propertyInfo.GetValue(args));
			//			}
			//		}
			//	}

			//};
			eventSource.BuildStarted += (sender, args) => Console.WriteLine(args.Message);
			eventSource.ProjectStarted += (sender, args) => Console.WriteLine(args.Message);
			eventSource.BuildFinished += (sender, args) => Console.WriteLine(args.Message);
			eventSource.ProjectFinished += (sender, args) => Console.WriteLine(args.Message);
			eventSource.MessageRaised += (sender, args) => {
				if (args.Importance >= MessageImportance.Normal) {
					Console.WriteLine(args.Message);
				}
			};
		}

		private static bool Ignore(BuildEventArgs args) {
			return (args is BuildMessageEventArgs && (args as BuildMessageEventArgs).Importance == MessageImportance.Low)  || !(args is BuildMessageEventArgs);
		}

		public void Shutdown() {}
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }
	}
}
