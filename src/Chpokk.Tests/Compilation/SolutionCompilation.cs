using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Arractas;
using CThru;
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
			var loggers = new ILogger[] { new TestLogger() };
			var targets = new string[] { "Build" };
			var globalProperties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
			//CThruEngine.AddAspect(new TraceAspect(info => info.TargetInstance is ILogger).DisplayFor<BuildEventArgs>(args => args.GetType().Name + ": " + args.Message));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("ItemFactory") && info.MethodName == "CreateItem"));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("Lookup") && info.MethodName.Contains("ctor"), 4).DisplayFor<IEnumerable<ProjectInstance>>(instances => instances.Count().ToString()));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("Lookup") && info.MethodName.Contains("ctor"), 4).DisplayFor(o => o is IEnumerable, o => "match"));
			//CThruEngine.AddAspect(new ExpandTracker());
			//CThruEngine.AddAspect(new CreateItemTracker());
			//CThruEngine.AddAspect(new DebugAspect(info => info.MethodName == "ExpandSingleItemVectorExpressionIntoItems"));
			//CThruEngine.AddAspect(new TraceAspect(info => info.MethodName == "ExpandSingleItemVectorExpressionIntoItems"));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("MSBuild") && info.MethodName.Contains("Projects")));
			//CThruEngine.AddAspect(new DebugAspect(info => info.TypeName.EndsWith("ProjectTaskInstance") ));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.Contains("SolutionParser"), 4));
			//CThruEngine.AddAspect(new TraceAspect(info => info.TypeName.EndsWith("BuildRequestConfiguration") && info.MethodName.Contains("ctor")).DisplayFor <BuildRequestData>(data => data.ProjectFullPath));
			//CThruEngine.AddAspect(new TraceResultAspect(info => info.MethodName == "ExpandSingleItemVectorExpressionIntoItems"));
			//CThruEngine.AddAspect(new TraceResultAspect(info => info.MethodName == "ExpandIntoTaskItemsLeaveEscaped"));
			//CThruEngine.StartListening();
			var projectCollection = new ProjectCollection(globalProperties, loggers, null, ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry, 1, false);
			//var projekt =
			//	ProjectCollection.GlobalProjectCollection.LoadProject(
			//		@"D:\Projects\Chpokk\src\Chpokk.Tests\Chpokk.Tests.csproj");
			//projectCollection = ProjectCollection.GlobalProjectCollection;
			//projectCollection.RegisterLogger(logger);
			var project =
				ProjectRootElement.Open(
					Context.ProjectFilePath);
			if (project.DefaultTargets.IsEmpty()) {
				project.DefaultTargets = "Build";
				project.Save();
			}
			project.DefaultTargets.ShouldNotBe(string.Empty);
			var requestData = new BuildRequestData(solutionPath, globalProperties, null, targets, null);
			var parameters = new BuildParameters(projectCollection)
				{
					Loggers = loggers, //need this to have any output at all
					ToolsetDefinitionLocations = ToolsetDefinitionLocations.ConfigurationFile | ToolsetDefinitionLocations.Registry
				};
			BuildManager.DefaultBuildManager.Build(parameters, requestData);			
		}

		public override void CleanUp() {
			base.CleanUp();
			DirectoryHelper.DeleteDirectory(@"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\uluhonolulu_Google\Repositories\CompileSolution\CompileSolution\bin\");
		}

		class CreateItemTracker: CommonAspect {
			public CreateItemTracker() : base(info => info.TypeName.Contains("ItemFactory") && info.MethodName == "CreateItem") { }

			public override void MethodBehavior(DuringCallbackEventArgs e) {
				base.MethodBehavior(e);
				if (((string)e.ParameterValues[0]).StartsWith("CurrentSolutionConfigurationContents")) {
					Debugger.Break();
				}
			}
		}


		class ExpandTracker: CommonAspect {
			public ExpandTracker() : base(info => info.TypeName.EndsWith("ItemExpander") && info.MethodName == "ExpandSingleItemVectorExpressionIntoItems") { }

			public override void MethodBehavior(DuringCallbackEventArgs e) {
				base.MethodBehavior(e);
				if (((string)e.ParameterValues[1]).Equals("@(ProjectReference)")) {
					Debugger.Break();
				}
			}
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
					//var targetAgs = args as TargetFinishedEventArgs;
					//if (targetAgs != null) {
					//	Console.WriteLine("TargetFile: " + targetAgs.TargetFile);
					//}

					//var taskArgs = args as TaskStartedEventArgs;
					//if (taskArgs != null) {
					//	//Console.WriteLine(taskArgs.ProjectFile);
					//}
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
		{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}.Debug|Any CPU.ActiveCfg = Debug|Any CPU
		{{7F5E6663-10AD-4671-80E6-8095EE4BC6F9}}.Debug|Any CPU.Build.0 = Debug|Any CPU
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal
";
		}

		protected override string GetProjectFileContent() {
			return 
@"<?xml version=""1.0"" encoding=""utf-8""?>
<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
  <PropertyGroup>
    <OutputType>Library</OutputType>
    <OutputPath>bin\Debug\</OutputPath>
    <RootNamespace>{1}</RootNamespace>
    <AssemblyName>{1}</AssemblyName>
  </PropertyGroup>
	<ItemGroup>
		<Compile Include=""{0}"" />
	</ItemGroup>
	<Import Project=""$(MSBuildToolsPath)\Microsoft.CSharp.targets"" />
</Project>".ToFormat(CODEFILE_NAME, PROJECT_NAME);
		}
	}
}
