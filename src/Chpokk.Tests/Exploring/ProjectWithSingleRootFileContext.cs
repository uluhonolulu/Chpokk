using System;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class ProjectWithSingleRootFileContext : RepositoryFolderContext {
		public readonly string SOLUTION_FOLDER = "src";
		//public string PROJECT_ROOT = "root";
		public readonly string PROJECT_NAME = "ProjectName";
		public readonly string PROJECT_PATH = @"ProjectName\ProjectName.csproj";
		public readonly string FILE_NAME = "Class1.cs";
		public string PROJECT_FILE_CONTENT =
			@"<?xml version=""1.0"" encoding=""utf-8""?>
				<Project ToolsVersion=""4.0"" DefaultTargets=""Build"" xmlns=""http://schemas.microsoft.com/developer/msbuild/2003"">
				  <ItemGroup>
					<Compile Include=""Class1.cs"" />
				  </ItemGroup>
				</Project>";

		public string SolutionFolder { get; set; }

		public override void Create() {
			base.Create();
			SolutionFolder = FileSystem.Combine(RepositoryRoot, SOLUTION_FOLDER);
			var projectPath = FileSystem.Combine(SolutionFolder, PROJECT_PATH);
			Console.WriteLine("Writing to " + projectPath);
			Container.Get<IFileSystem>().WriteStringToFile(projectPath, PROJECT_FILE_CONTENT);
		}
	}
}