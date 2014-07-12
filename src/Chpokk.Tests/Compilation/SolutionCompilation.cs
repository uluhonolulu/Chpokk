using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using CThru.BuiltInAspects;
using Chpokk.Tests.Exploring;
using Chpokk.Tests.Infrastructure;
using Chpokk.Tests.Intellisense;
using ChpokkWeb.Features.Compilation;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Microsoft.Build.Construction;
using Microsoft.Build.Evaluation;
using Microsoft.Build.Execution;
using Microsoft.Build.Framework;
using Microsoft.Build.Logging;
using Shouldly;
using FubuCore;
using System.Linq;

namespace Chpokk.Tests.Compilation {
	[TestFixture]
	public class SolutionCompilation : BaseCommandTest<CompilableSolutionContext> {
		//track this:  	Microsoft.Build.dll!Microsoft.Build.Evaluation.Expander<Microsoft.Build.Execution.ProjectPropertyInstance,Microsoft.Build.Execution.ProjectItemInstance>.ExpandIntoTaskItemsLeaveEscaped(string expression = "BuildingSolutionFile=true; CurrentSolutionConfigurationContents=$(CurrentSolutionConfigurationContents); SolutionDir=$(SolutionDir); SolutionExt=$(SolutionExt); SolutionFileName=$(SolutionFileName); SolutionName=$(SolutionName); SolutionPath=$(SolutionPath)", Microsoft.Build.Evaluation.ExpanderOptions options = ExpandCustomMetadata | ExpandBuiltInMetadata | ExpandProperties | ExpandItems, Microsoft.Build.Shared.IElementLocation elementLocation = {Microsoft.Build.Construction.ElementLocation.SmallElementLocation})	
		// figure why CurrentSolutionConfigurationContents expands to an empty xml

//		lookup.lookupScopes --> take 4th (Description = "Lookup()", Items contains project instance
//gotta trace constructor of Lookup: Lookup(ItemDictionary<ProjectItemInstance> projectItems, PropertyDictionary<ProjectPropertyInstance> properties, IDictionary<string, object> globalsForDebugging)
		[Test]
		public void Test() {
			var outputPath = Context.ProjectFolder.AppendPath(@"bin\Debug").AppendPath(Context.PROJECT_NAME + ".dll");
			File.Exists(outputPath).ShouldBe(true);
			//File.Exists(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution\bin\Debug\CompileSolution.exe").ShouldBe(true);
		}

		public override void Act() {
			Console.WriteLine();
			var solutionPath =
				@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution.sln";
			solutionPath = Context.SolutionPath;
			//var logger = Context.Container.Get<ChpokkLogger>();
			//logger.ConnectionId = "fakeID";
			//logger.ConnectionId = "FakeID";
			ILogger logger = new TestLogger();
			var solutionCompiler = Context.Container.Get<SolutionCompiler>();
			solutionCompiler.CompileSolution(solutionPath, logger);
		}

		public override void CleanUp() {
			base.CleanUp();
			DirectoryHelper.DeleteDirectory(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution\bin\");
		}


		class TestLogger: ILogger {
			public void Initialize(IEventSource eventSource) {
				Verbosity = LoggerVerbosity.Diagnostic;
				eventSource.AnyEventRaised += (sender, args) =>
				{
					//Console.WriteLine(args.Message + " (" + args.GetType().Name + ")");
					var errorArgs = args as BuildErrorEventArgs;
					if (errorArgs != null) {
						Console.WriteLine(errorArgs.Subcategory);
						Console.WriteLine(errorArgs.File + ", line " + errorArgs.LineNumber);
						throw new Exception(args.Message);
					}
					var warningArgs = args as BuildWarningEventArgs;
					if (warningArgs != null) {
						Console.WriteLine(warningArgs.Subcategory);
						Console.WriteLine(warningArgs.File + ", line " + warningArgs.LineNumber);
						Console.WriteLine(args.Message + " (" + warningArgs.SenderName + ")");
						
					}
				};
			}
			public void Shutdown() {}
			public LoggerVerbosity Verbosity { get; set; }
			public string Parameters { get; set; }
		}
	}

	public class CompilableSolutionContext: SolutionWithProjectAndClassFileContext {
		protected override string GetSolutionContent() {
			return @"Microsoft Visual Studio Solution File, Format Version 12.00
Project(""{{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}}"") = ""{0}"", ""{1}"", ""{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|Any CPU = Debug|Any CPU
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}.Debug|Any CPU.ActiveCfg = Debug|x86
		{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}.Debug|Any CPU.Build.0 = Debug|x86
	EndGlobalSection
EndGlobal
";
		}

		protected override string GetProjectFileContent() {
			return 
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>bin\Debug\</OutputPath>
    <RootNamespace>{1}</RootNamespace>
  </PropertyGroup>
	<ItemGroup>
		<Compile Include=""{0}"" />
	</ItemGroup>
	<Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>".ToFormat(CODEFILE_NAME, PROJECT_NAME);
		}
	}
}
