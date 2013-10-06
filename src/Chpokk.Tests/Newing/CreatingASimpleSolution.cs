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
			CreateSolutionWithProject(repositoryManager, name, appRoot, fileSystem);
			//{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC} -- C#
			//{F184B08F-C81C-45F6-A57F-5ABD9991F28F} -- VB.Net
			//{349C5851-65DF-11DA-9384-00065B846F21} -- Web app
			//{E24C65DC-7377-472B-9ABA-BC803B73C61A} -- Web site


			//create a project
			var projectPath = RepositoryManager.GetAbsolutePathFor(name, appRoot, Path.Combine(name, name + ".csproj"));
			var rootElement = ProjectRootElement.Create();
			rootElement.AddImport(@"$(MSBuildToolsPath)\Microsoft.CSharp.targets");
			var outputType = "Library"; //can be EXE
			rootElement.AddProperty("OutputType", outputType);
			rootElement.Save(projectPath);

			//TODO: cleanup
		}



		public static void CreateSolutionWithProject(RepositoryManager repositoryManager, string name, string appRoot,
		                                             IFileSystem fileSystem) {
			var solutionPath = repositoryManager.GetAbsolutePathFor(name, appRoot, name + ".sln");
			CreateEmptySolution(fileSystem, solutionPath);
			AddProjectToSolution(name, fileSystem, solutionPath);
		}

		public static void AddProjectToSolution(string name, IFileSystem fileSystem, string solutionPath) {
			var projectTypeGuid = "FAE04EC0-301F-11D3-BF4B-00C04F79EFBC";
			var solutionContent = fileSystem.ReadStringFromFile(solutionPath);
			solutionContent += Environment.NewLine;
			solutionContent += @"Project(""{{{1}}}"") = ""{0}"", ""{0}\{0}.csproj"", ""{{{2}}}""
EndProject".ToFormat(name, projectTypeGuid, Guid.NewGuid());
			fileSystem.WriteStringToFile(solutionPath, solutionContent);
		}

		public static void CreateEmptySolution(IFileSystem fileSystem, string solutionPath) {
			const string emptySolutionContent = @"Microsoft Visual Studio Solution File, Format Version 11.00
# Visual Studio 2010";
			fileSystem.WriteStringToFile(solutionPath, emptySolutionContent);
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
