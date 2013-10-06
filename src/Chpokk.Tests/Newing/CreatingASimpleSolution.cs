using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
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
	public class CreatingASimpleSolution : BaseCommandTest<CreatingASimpleSolution.RepositoryCleanupContext> {
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
			var parser = new SolutionParser();
			var projects = parser.GetProjectItems(File.ReadAllText(SolutionPath), SolutionPath);
			projects.Count().ShouldBe(1);
			projects.First().Name.ShouldBe(NAME);
		}

		[Test, DependsOn("TheSolutionFileHasAProjectOfTheSameName")]
		public void CreatesAProjectFileInASubfolder() {
			File.Exists(ProjectPath).ShouldBe(true);
		}

		private string ProjectPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, NAME + ".csproj")); }
		}

		[Test, DependsOn("CreatesAProjectFileInASubfolder")]
		public void ProjectCanBeCompiled() {
			new Project(ProjectPath).Build(new ConsoleBuildLogger()).ShouldBe(true);
			var binPath = RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, Path.Combine(NAME, "bin"));
			Directory.Exists(binPath).ShouldBe(true);
		}
	

		public override void Act() {
			var name = NAME;
			var appRoot = Context.AppRoot;
			//create a repo
			//Directory.CreateDirectory(RepositoryManager.GetAbsolutePathFor(name, appRoot));

			//create a solution
			var repositoryManager = RepositoryManager;
			var fileSystem = Context.Container.Get<IFileSystem>();
			var solutionFileLoader = Context.Container.Get<SolutionFileLoader>();
			solutionFileLoader.CreateSolutionWithProject(repositoryManager, name, appRoot, fileSystem);
			//{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC} -- C#
			//{F184B08F-C81C-45F6-A57F-5ABD9991F28F} -- VB.Net
			//{349C5851-65DF-11DA-9384-00065B846F21} -- Web app
			//{E24C65DC-7377-472B-9ABA-BC803B73C61A} -- Web site


			//create a project
			var outputType = "Library"; //can be EXE
			var repositoryName = name;
			var projectPath = repositoryManager.GetAbsolutePathFor(repositoryName, appRoot, Path.Combine(name, name + ".csproj"));
			Context.Container.Get<ProjectParser>().CreateProjectFile(outputType, projectPath);

			//TODO: cleanup
		}


		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }

		private string SolutionPath {
			get { return RepositoryManager.GetAbsolutePathFor(NAME, Context.AppRoot, NAME + ".sln"); }
		}

		class ConsoleBuildLogger: ILogger {
			public void Initialize(IEventSource eventSource) {
				Verbosity = LoggerVerbosity.Minimal;
				eventSource.ErrorRaised += (sender, args) => Console.WriteLine(args.Message);
			}
			public void Shutdown() {}
			public LoggerVerbosity Verbosity { get; set; }
			public string Parameters { get; set; }
		}

		public class RepositoryCleanupContext : SimpleConfiguredContext, IDisposable {
			public void Dispose() {
				var repositoryManager = Container.Get<RepositoryManager>();
				var folder = repositoryManager.GetAbsolutePathFor(CreatingASimpleSolution.NAME, AppRoot);
				DirectoryHelper.DeleteDirectory(folder);
			}
		}	
	}

}
