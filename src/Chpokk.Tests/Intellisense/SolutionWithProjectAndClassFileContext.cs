using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chpokk.Tests.Exploring;
using FubuCore;

namespace Chpokk.Tests.Intellisense {
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
