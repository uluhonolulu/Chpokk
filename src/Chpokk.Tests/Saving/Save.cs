using System;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Remotes.SaveCommit;
using FubuCore;
using LibGit2Sharp;
using MbUnit.Framework;
using LibGit2Sharp.Tests.TestHelpers;
using System.Collections.Generic;
using System.Linq;

namespace Chpokk.Tests.Saving {
	[TestFixture]
	public class Save : BaseCommandTest<PhysicalCodeFileInRepositoryContext> {
		private const string NEW_CONTENT = "---";
		[Test]
		public void ShouldHaveFileContentsChangedToNew() {
			var system = Context.Container.Get<FileSystem>();
			system.FileExists(Context.FilePath).ShouldBeTrue();
			var content = system.ReadStringFromFile(Context.FilePath);
			content.ShouldEqual(NEW_CONTENT);
		}

		[Test]
		public void ShouldntCommitWhenNotAskedTo() {
			using (var repo = new Repository(Context.RepositoryRoot)) {
				repo.Commits.Count().ShouldEqual(0);
			}
			
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
			using (var repo = new Repository(Context.RepositoryRoot)) {
				repo.Index.Stage(Context.FilePath);				
			}
			controller.Save(new SaveCommitModel { RepositoryName = Context.REPO_NAME, Content = NEW_CONTENT, PathRelativeToRepositoryRoot = pathRelativeToRepositoryRoot, PhysicalApplicationPath = Path.GetFullPath(@".."), DoCommit = true, CommitMessage = "doesntmater" });

		}

		[Test]
		public void ShouldCommitWhenAskedSo() {
			using (var repo = new Repository(Context.RepositoryRoot)) {
				//repo.Index.Stage(Context.FilePath);
				repo.Commits.Count().ShouldEqual(1);
				//Console.WriteLine();
				//repo.Commits.First()[Context.FilePath].Path
			}
		}

		[Test, DependsOn("ShouldCommitWhenAskedSo")]
		public void CommitShouldContainTheChangedFile() {
			using (var repo = new Repository(Context.RepositoryRoot)) {
				var commit = repo.Head.Tip;
				var blob = (Blob) commit[@"src\ProjectName\Class1.cs"].Target;
				string content = Encoding.UTF8.GetString(blob.Content);
				content.ShouldEqual("---");
			}			
		}
	}
}
