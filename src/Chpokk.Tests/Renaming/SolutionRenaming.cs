using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Exploring.Rename;
using FubuCore;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Renaming {
	[TestFixture]
	public class SolutionRenaming : BaseCommandTest<EmptySlnFileContext> {
		[Test]
		public void NewFileExists() {
			var fileSystem = Context.Container.Get<IFileSystem>();
			fileSystem.FileExists(NewFilePath).ShouldBe(true);
		}

		[Test]
		public void OldFileDoesntExist() {
			var fileSystem = Context.Container.Get<IFileSystem>();
			fileSystem.FileExists(OldFilePath).ShouldBe(false);
		}

		public override void Act() {
			var endpoint = Context.Container.Get<RenameEndpoint>();
			File.Exists(OldFilePath).ShouldBe(true);
			endpoint.DoIt(new RenameInputModel
				{
					RepositoryName = Context.REPO_NAME,
					PathRelativeToRepositoryRoot = Context.RelativeSolutionPath,
					NewFileName = "new_" + Context.SolutionFileName
				});
		}

		private string OldFilePath {
			get { return Context.SolutionPath; }
		}

		private string NewFilePath {
			get {
				return OldFilePath.Replace(Context.SolutionFileName, "new_" + Context.SolutionFileName);
			}
		}
	}
}
