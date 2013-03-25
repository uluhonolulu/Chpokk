using System;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public abstract class ProjectFileContext : RepositoryFolderContext {
		public readonly string SOLUTION_FOLDER = "src";
		public readonly string PROJECT_NAME = "ProjectName";
		public readonly string PROJECT_PATH = @"ProjectName\ProjectName.csproj";
		public readonly string FILE_NAME = "Class1.cs";
		public string SolutionFolder { get; set; }
		public string ProjectPath { get; private set; }
		public abstract string ProjectFileContent { get; }

		public override void Create() {
			base.Create();
			SolutionFolder = FileSystem.Combine(RepositoryRoot, SOLUTION_FOLDER);
			ProjectPath = FileSystem.Combine(SolutionFolder, PROJECT_PATH);
			Console.WriteLine("Writing to " + ProjectPath);
			Container.Get<IFileSystem>().WriteStringToFile(ProjectPath, ProjectFileContent);
		}
	}
}