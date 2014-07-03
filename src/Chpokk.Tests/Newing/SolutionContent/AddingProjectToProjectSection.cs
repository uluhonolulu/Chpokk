using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;
using FubuCore;

namespace Chpokk.Tests.Newing.SolutionContent {
	[TestFixture]
	public class AddingProjectToProjectSection: BaseQueryTest<SimpleConfiguredContext, string> {
		private Guid projectGuid = Guid.NewGuid();
		[Test]
		public void PlacesProjectDataInTheProjectSection() {
			Console.WriteLine(Result);
			Assert.Contains(Result, @"Project(""ProjectTypeGuid"") = ""ProjectName"", ""ProjectName\ProjectName.csproj"", ""{{{0}}}""".ToFormat(projectGuid));
		}

		public override string Act() {
			var parser = Context.Container.Get<ProjectParser>();
			var solutionContent = @"Microsoft Visual Studio Solution File, Format Version 12.00
# Visual Studio 2013
VisualStudioVersion = 12.0.30110.0
MinimumVisualStudioVersion = 10.0.40219.1
Project(""{FAE04EC0-301F-11D3-BF4B-00C04F79EFBC}"") = ""CompileSolution"", ""CompileSolution\CompileSolution.csproj"", ""{6B9D37AB-EEC9-4AF2-AF04-1CD65C2076EE}""
EndProject
Global
	GlobalSection(SolutionConfigurationPlatforms) = preSolution
		Debug|x86 = Debug|x86
	EndGlobalSection
	GlobalSection(ProjectConfigurationPlatforms) = postSolution
		{6B9D37AB-EEC9-4AF2-AF04-1CD65C2076EE}.Debug|x86.ActiveCfg = Debug|x86
		{6B9D37AB-EEC9-4AF2-AF04-1CD65C2076EE}.Debug|x86.Build.0 = Debug|x86
	EndGlobalSection
	GlobalSection(SolutionProperties) = preSolution
		HideSolutionNode = FALSE
	EndGlobalSection
EndGlobal";
			parser.AddProjectSectionToSolutionContent("ProjectName", "ProjectTypeGuid", ".csproj", projectGuid, ref solutionContent);
			return solutionContent;
		}
	}
}
