using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FubuCore;

namespace Chpokk.Tests.Exploring {
	public class PhysicalCodeFileContext : ProjectWithSingleRootFileContext {
		public string FilePath { get; private set; }
		public override void Create() {
			base.Create();
			FilePath = FileSystem.Combine(SolutionFolder, PROJECT_NAME, FILE_NAME);
			Console.WriteLine("Writing to " + FilePath);
			Container.Get<IFileSystem>().WriteStringToFile(FilePath, "");

		}

	}
}
