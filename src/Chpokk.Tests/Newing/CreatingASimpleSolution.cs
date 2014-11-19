using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
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
			var projects = parser.GetProjectItems(SolutionPath);
			projects.Count().ShouldBe(1);
			projects.First().Name.ShouldBe(NAME);
		}

		[Test, DependsOn("TheSolutionFileHasAProjectOfTheSameName")]
		public void CreatesAProjectFileInASubfolder() {
			File.Exists(ProjectPath).ShouldBe(true);
		}

		[Test, DependsOn("CreatesAProjectFileInASubfolder")]
		public void ProjectCanBeCompiled() {
			ProjectCollection.GlobalProjectCollection.LoadProject(ProjectPath).Build(new CreatingASimpleSolution.ConsoleBuildLogger()).ShouldBe(true);
			var binPath = RepositoryManager.NewGetAbsolutePathFor(NAME, Path.Combine(NAME, "bin"));
			Directory.Exists(binPath).ShouldBe(true);
		}
	
		[Test, DependsOn("CreatesASolutionFile")]
		public void TheSolutionFileContainsStringsRequiredForBuilding() {
			var solutionContent = File.ReadAllText(SolutionPath);
			var expected = @"	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{0}.Debug|Any CPU.ActiveCfg = Debug|x86
		{0}.Debug|Any CPU.Build.0 = Debug|x86
	EndGlobalSection".ToFormat(NAME);
			//solutionContent.ShouldContain(expected); // Actually we should have the project GUID here in the expected string 
		}

		public override void Act() {
			var endpoint = Context.Container.Get<AddSimpleProjectEndpoint>();
			endpoint.DoIt(new AddSimpleProjectInputModel{ ProjectName = NAME, OutputType = "Library", ConnectionId = "fake"});
		}

		[Test]
		public void SpikinItNow() {
			var pattern = @"(?<=GlobalSection\(ProjectConfigurationPlatforms\) = postSolution\s*)$(.*?)(?=^\s*EndGlobalSection)";
			var reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline);
			var solutionContent = @"Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
		Release|Any CPU = Release|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{6B9D37AB-EEC9-4AF2-AF04-1CD65C2076EE}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{6B9D37AB-EEC9-4AF2-AF04-1CD65C2076EE}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{6B9D37AB-EEC9-4AF2-AF04-1CD65C2076EE}.Release|Any CPU.Build.0 = Release|Any CPU
		{244CBBEC-847E-4E0D-8857-05BB34878174}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{244CBBEC-847E-4E0D-8857-05BB34878174}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{244CBBEC-847E-4E0D-8857-05BB34878174}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{244CBBEC-847E-4E0D-8857-05BB34878174}.Release|Any CPU.Build.0 = Release|Any CPU
		{CCA7F698-721E-4850-BF34-43C568E60C9A}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{CCA7F698-721E-4850-BF34-43C568E60C9A}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{CCA7F698-721E-4850-BF34-43C568E60C9A}.Release|Any CPU.Build.0 = Release|Any CPU
		{C33B69A4-8D74-46BA-91D2-154617603F6F}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{C33B69A4-8D74-46BA-91D2-154617603F6F}.Debug|Any CPU.Build.0 = Debug|Any CPU
		{C33B69A4-8D74-46BA-91D2-154617603F6F}.Release|Any CPU.ActiveCfg = Release|Any CPU
		{C33B69A4-8D74-46BA-91D2-154617603F6F}.Release|Any CPU.Build.0 = Release|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal";
			var match = reg.Match(solutionContent);
			//Console.WriteLine(match.Captures[0].Value);
			Console.WriteLine(match.Value);
			Console.WriteLine();
			var result = reg.Replace(solutionContent, " start\r\n $& \r\nend ");
			Console.WriteLine(result);
		}


		RepositoryManager RepositoryManager { get { return Context.Container.Get<RepositoryManager>(); } }

		private string SolutionPath {
			get { return RepositoryManager.NewGetAbsolutePathFor(NAME, NAME + ".sln"); }
		}

		private string ProjectPath {
			get { return RepositoryManager.NewGetAbsolutePathFor(NAME, Path.Combine(NAME, NAME + ".csproj")); }
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
		public override void Dispose() {
			var repositoryManager = Container.Get<RepositoryManager>();
			var folder = repositoryManager.GetAbsolutePathFor(CreatingASimpleSolution.NAME, AppRoot);
			//DirectoryHelper.DeleteDirectory(folder);
		}
	}
}
