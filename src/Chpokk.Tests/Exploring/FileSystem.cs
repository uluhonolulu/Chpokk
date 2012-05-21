﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Repa;
using Gallio.Framework;
using HtmlTags;
using LibGit2Sharp.Tests.TestHelpers;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class SingleFile : BaseQueryTest<SingleFileContext, IList<RepositoryItem>> {

		[Test]
		public void HasOneRootItem() { // we have one root item, like "File system"
			Assert.AreEqual(1, Result.Count);
		}

		[Test]
		public void HasOneChildItemWithNameAndPathSetToThoseOfTheExistingFile() {
			var root = Result.First();
			root.Children.Each(item => Console.WriteLine(item.Name));
			Assert.AreEqual(1, root.Children.Count);
			var child = root.Children.First();
			Assert.AreEqual(Context.FileName, child.Name);
			Assert.AreEqual(Context.FilePathRelativeToRepositoryRoot, child.PathRelativeToRepositoryRoot);
		}

		public override IList<RepositoryItem> Act() {
			var controller = Context.Container.Get<ContentController>();
			return controller.GetFileList(new FileListInputModel {PhysicalApplicationPath = Path.GetFullPath("..")}).Items;
		}
	}

	[TestFixture]
	public class FileHierarchy : BaseQueryTest<RootFileAndFileInSubfolderContext, IList<RepositoryItem>> {

		[Test]
		public void HasTwoChildItems() {
			var root = Result.First();
			root.Children.Each(item => Console.WriteLine(item.Name));
			Assert.AreEqual(2, root.Children.Count);
		}

		[Test]
		public void FirstItemHasSameNameAndPathAsSubfolder() {
			var folderItem = Result.First().Children.First();
			Assert.AreEqual(RootFileAndFileInSubfolderContext.FOLDER_NAME, folderItem.Name);
			Assert.AreEqual(@"\" + RootFileAndFileInSubfolderContext.FOLDER_NAME, folderItem.PathRelativeToRepositoryRoot);
		}

		[Test]
		public void GrandChildItemHasSameNameAndPathAsFileInSubfolder() {
			var grandchild = Result.First().Children.First().Children.First();
			Assert.AreEqual(Context.OtherFileName, grandchild.Name);
			Assert.AreEqual(Context.OtherFilePathRelativeToRepositoryRoot, grandchild.PathRelativeToRepositoryRoot);
		}

		public override IList<RepositoryItem> Act() {
			var controller = Context.Container.Get<ContentController>();
			return controller.GetFileList(new FileListInputModel { PhysicalApplicationPath = Path.GetFullPath("..") }).Items;
		}
	}

	public class SingleFileContext : SimpleConfiguredContext, IDisposable {
		public string FileName { get; set; }
		public string FilePath { get; private set; }
		public string RepositoryRoot { get; private set; }
		public string FilePathRelativeToRepositoryRoot {
			get { return FilePath.Substring(RepositoryRoot.Length); }
		}
		public override void Create() {
			base.Create();
			RepositoryRoot = Path.Combine(Path.GetFullPath(@".."), RepositoryInfo.Path);
			if (!Directory.Exists(RepositoryRoot))
				Directory.CreateDirectory(RepositoryRoot);
			FileName = Guid.NewGuid().ToString();
			FilePath = Path.Combine(RepositoryRoot, FileName);
			File.Create(FilePath).Dispose();
		}

		public void Dispose() {
			DirectoryHelper.DeleteDirectory(RepositoryRoot);
		}
	}

	public class RootFileAndFileInSubfolderContext : SingleFileContext {
		public const string FOLDER_NAME = "Subfolder";
		public string OtherFileName { get; set; }
		public string OtherFilePath { get; set; }
		public string OtherFilePathRelativeToRepositoryRoot {
			get { return OtherFilePath.Substring(RepositoryRoot.Length); }
		}
		public override void Create() {
			base.Create();
			var subfolderpath = Path.Combine(this.RepositoryRoot, FOLDER_NAME);
			Directory.CreateDirectory(subfolderpath);
			OtherFileName = Guid.NewGuid().ToString();
			OtherFilePath = Path.Combine(subfolderpath, OtherFileName);
			File.Create(OtherFilePath).Dispose();
		}
	}
}
