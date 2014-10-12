using System;
using Chpokk.Tests.Exploring;
using FubuCore;

namespace UnitTests.Roslynson {
	public class SolutionWithProjectAndClassFileContext : SolutionAndProjectFileWithSingleEntryContext {
		public string CLASS_CONTENT = @"public class A {
									public void B(){
									}
								}";

		public override void Create() {
			base.Create();
			var fileSystem = Container.Get<FileSystem>();
			Console.WriteLine("Writing file to " + ClassFilePath);
			fileSystem.WriteStringToFile(ClassFilePath, CLASS_CONTENT);
		}

		public string ClassFilePath {
			get { return FileSystem.Combine(ProjectFolder, CODEFILE_NAME); }
		}

		public string ClassFileRelativePath {
			get { return ClassFilePath.PathRelativeTo(this.RepositoryRoot); }
		}
	}
}
