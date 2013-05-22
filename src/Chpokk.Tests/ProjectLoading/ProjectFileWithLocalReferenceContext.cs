using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Chpokk.Tests.Exploring;
using FubuCore;

namespace Chpokk.Tests.ProjectLoading {
	public class ProjectFileWithLocalReferenceContext : ProjectFileContext {
		public override string ProjectFileContent {
			get {
				var hintPath = @"..\..\..\..\..\..\packages\Bottles.1.0.0.443\lib\Bottles.dll";
				var assemblyPath = Path.GetFullPath(Path.Combine(this.ProjectPath.ParentDirectory(), hintPath)) ;
				if (!File.Exists(assemblyPath)) {
					throw new Exception("File " + assemblyPath + " does not exist!");
				}
				return @"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Reference Include=""Bottles, Version=0.9.1.0, Culture=neutral, processorArchitecture=MSIL"">
					  <SpecificVersion>False</SpecificVersion>
					  <HintPath>{0}</HintPath>
					</Reference>
					</ItemGroup>
				</Project>".ToFormat(hintPath);
			}
		}
	}
}
