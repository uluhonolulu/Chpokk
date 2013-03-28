using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chpokk.Tests.Exploring;

namespace Chpokk.Tests.ProjectLoading {
	public class ProjectFileWithLocalReferenceContext : ProjectFileContext {
		public override string ProjectFileContent {
			get {
				return @"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Reference Include=""Bottles, Version=0.9.1.0, Culture=neutral, processorArchitecture=MSIL"">
					  <SpecificVersion>False</SpecificVersion>
					  <HintPath>..\..\..\..\..\packages\Bottles.0.9.1.274\lib\Bottles.dll</HintPath>
					</Reference>
					</ItemGroup>
				</Project>";
			}
		}
	}
}
