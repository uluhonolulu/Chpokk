﻿using System;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Exploring;
using ChpokkWeb.Features.Files;
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
			var controller = Context.Container.Get<SaveEndpoint>();
			const string pathRelativeToRepositoryRoot = @"src\ProjectName\Class1.cs";
			controller.DoIt(new SaveCommitInputModel { RepositoryName = Context.REPO_NAME, Content = NEW_CONTENT, PathRelativeToRepositoryRoot = pathRelativeToRepositoryRoot });
		}
	}

	public class Commit : BaseCommandTest<PhysicalCodeFileInRepositoryContext> {
		private const string NEW_CONTENT = "---";
		public override void Act() {
			var controller = Context.Container.Get<SaveCommitEndpoint>();
			const string pathRelativeToRepositoryRoot = @"src\ProjectName\Class1.cs";
			//Context.FakeSecurityContext.UserName = "ulu";
			using (var repo = new Repository(Context.RepositoryRoot)) {
				repo.Index.Stage(Context.FilePath);				
			}
			controller.SaveCommit(new SaveCommitInputModel { RepositoryName = Context.REPO_NAME, Content = NEW_CONTENT, PathRelativeToRepositoryRoot = pathRelativeToRepositoryRoot, CommitMessage = "doesntmater" });

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
				string content = blob.GetContentText();
				content.ShouldEqual("---");
			}			
		}
	}
}
