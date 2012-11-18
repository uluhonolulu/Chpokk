using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using LibGit2Sharp.Tests.TestHelpers;

namespace Chpokk.Tests.Saving {
	[TestFixture]
	public class Save : BaseCommandTest<PhysicalCodeFileContext> {
		private const string NEW_CONTENT = "---";
		[Test]
		public void ShouldHaveFileContentsChangedToNew() {
			var system = Context.Container.Get<FileSystem>();
			system.FileExists(Context.FilePath).ShouldBeTrue();
			var content = system.ReadStringFromFile(Context.FilePath);
			content.ShouldEqual(NEW_CONTENT);
		}

		public override void Act() {
			var controller = Context.Container.Get<SaveCommitController>();
			controller.Save(new SaveCommitModel() {Content = NEW_CONTENT, FilePath = Context.FilePath});
			//var system = Context.Container.Get<FileSystem>();
			//system.WriteStringToFile(Context.FilePath, NEW_CONTENT);
		}
	}

	public class SaveCommitModel {
		public string FilePath { get; set; }
		public string Content { get; set; }
	}

	public class SaveCommitController {
		private readonly FileSystem _fileSystem;
		public SaveCommitController(FileSystem fileSystem) {
			_fileSystem = fileSystem;
		}

		public void Save(SaveCommitModel saveCommitModel) {
			_fileSystem.WriteStringToFile(saveCommitModel.FilePath, saveCommitModel.Content);
		}
	}
}
