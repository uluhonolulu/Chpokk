using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using ChpokkWeb.Features.ProjectManagement.AddSimpleProject;
using ChpokkWeb.Features.RepositoryManagement;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using LibGit2Sharp.Tests.TestHelpers;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Shouldly;
using System.Linq;

namespace Chpokk.Tests.Newing {


	[TestFixture]
	public class CreatingASimpleSolution : BaseCommandTest<RepositoryCleanupContext> {
		public const string NAME = "Carramba";
		[Test]
		public void CreatesARepository() {
			RepositoryManager.RepositoryNameIsValid(NAME, Context.AppRoot).ShouldBe(true);
		}

		[Test]
		public void CreatesASolutionFile() {
			File.Exists(SolutionPath).ShouldBe(true);
		}

		[Test, DependsOn("CreatesASolutionFile")]
		public void TheSolutionFileHasAProjectOfTheSameName() {
			var parser = Context.Container.Get<SolutionParser>();
			var projects = parser.ParseSolutionContent(SolutionPath);
			projects.Count().ShouldBe(1);
			projects.First().Name.ShouldBe(NAME);
		}

		[Test, DependsOn("TheSolutionFileHasAProjectOfTheSameName")]
		public void CreatesAProjectFileInASubfolder() {
			File.Exists(ProjectPath).ShouldBe(true);
		}

		[Test, DependsOn("CreatesAProjectFileInASubfolder")]
		public void ProjectCanBeCompiled() {
			new Project(ProjectPath).Build(new CreatingASimpleSolution.ConsoleBuildLogger()).ShouldBe(true);
			var binPath = RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, "bin"));
			Directory.Exists(binPath).ShouldBe(true);
		}
	

		public override void Act() {
			var endpoint = Context.Container.Get<AddSimpleProjectEndpoint>();
			endpoint.DoIt(new AddSimpleProjectInputModel{PhysicalApplicationPath = Context.AppRoot, RepositoryName = NAME, OutputType = "Library"});
		}


		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }

		private string SolutionPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, NAME + ".sln"); }
		}

		private string ProjectPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, NAME + ".csproj")); }
		}

		public class ConsoleBuildLogger: ILogger {
			public void Initialize(IEventSource eventSource) {
				Verbosity = LoggerVerbosity.Minimal;
				eventSource.ErrorRaised += (sender, args) => Console.WriteLine(args.Message);
			}
			public void Shutdown() {}
			public LoggerVerbosity Verbosity { get; set; }
			public string Parameters { get; set; }
		}
	}

	public class RepositoryCleanupContext : SimpleConfiguredContext, IDisposable {
		public void Dispose() {
			var repositoryManager = Container.Get<RepositoryManager>();
			var folder = repositoryManager.GetAbsolutePathFor(CreatingASimpleSolution.NAME, AppRoot);
			//DirectoryHelper.DeleteDirectory(folder);
		}
	}
}
