using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddItem;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Framework;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.ItemAdding {
	[TestFixture]
	public class ExecutingAddItemCommand : BaseCommandTest<SolutionAndProjectFileWithSingleEntryContext> {
		private Project _project;

		[Test]
		public void AddsAnItemToTheProjectFile() {
			//var project = new Project(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu\Chpokk-SampleSol\src\ConsoleApplication1\ConsoleApplication1.csproj");
			//project.Items.ShouldContain(item => item.EvaluatedInclude == "NewFile.cs");
			var project =
				ProjectCollection.GlobalProjectCollection.GetLoadedProjects(
					@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu\Chpokk-SampleSol\src\ConsoleApplication1\ConsoleApplication1.csproj")
				                 .Single();
			project.Items.ShouldContain(item => item.EvaluatedInclude.StartsWith("NewClass"));
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddItemEndpoint>();
			endpoint.DoIt(new AddItemInputModel()
			{
				PhysicalApplicationPath = Context.AppRoot,
				RepositoryName = Context.REPO_NAME,
				PathRelativeToRepositoryRoot = "Class1.cs"
			});
			//_project.Save();
			//project.Build(new ConsoleLogger());
		}
	}

	public class ConsoleLogger: ILogger {
		public void Initialize(IEventSource eventSource) {
			eventSource.AnyEventRaised += (sender, args) => Console.WriteLine(args.Message);
		}
		public void Shutdown() {}
		public LoggerVerbosity Verbosity { get; set; }
		public string Parameters { get; set; }
	}
}
