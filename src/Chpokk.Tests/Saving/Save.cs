using System;
using System.IO;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Editor.SaveCommit;
using FubuCore;
using LibGit2Sharp;
using MbUnit.Framework;
using LibGit2Sharp.Tests.TestHelpers;
using System.Collections.Generic;

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
			const string pathRelativeToRepositoryRoot = @"src\ProjectName\Class1.cs";
			controller.Save(new SaveCommitModel { RepositoryName = Context.REPO_NAME, Content = NEW_CONTENT, PathRelativeToRepositoryRoot = pathRelativeToRepositoryRoot, PhysicalApplicationPath = Path.GetFullPath(@"..") });
		}
	}

	public class Commit : BaseCommandTest<PhysicalCodeFileContext> {
		private const string NEW_CONTENT = "---";
		public override void Act() {
			var controller = Context.Container.Get<SaveCommitController>();
			const string pathRelativeToRepositoryRoot = @"src\ProjectName\Class1.cs";
			controller.Save(new SaveCommitModel { RepositoryName = Context.REPO_NAME, Content = NEW_CONTENT, PathRelativeToRepositoryRoot = pathRelativeToRepositoryRoot, PhysicalApplicationPath = Path.GetFullPath(@".."), DoCommit = true, CommitMessage = "doesntmater" });
		}

		[Test]
		public void ShouldCommitWhenAskedSo() {
			using (var repo = new Repository(Context.RepositoryRoot)) {
				repo.Commits.Each(commit => Console.WriteLine(commit.Message));
			}
			;
		}
	}
}
