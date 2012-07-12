using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Chpokk.Tests.Exploring;
using FubuCore;

namespace Chpokk.Tests.Intellisense {
	public class SolutionWithProjectAndClassFileContext : SolutionAndProjectFileWithSingleEntryContext {
		public override void Create() {
			base.Create();
			var classContent = @"public class A {
									public void B(){
									}
								}";
			var fileSystem = Container.Get<FileSystem>();
			var classFilePath = FileSystem.Combine(ProjectFolder, CODEFILE_NAME);
			Console.WriteLine("Writing file to " + classFilePath);
			fileSystem.WriteStringToFile(classFilePath, classContent);
		}
	}
}
