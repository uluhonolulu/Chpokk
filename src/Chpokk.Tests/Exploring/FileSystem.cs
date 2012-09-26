using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Exploring;
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
		public void HasOneChildItemWithNameAndPathSetToThoseOfTheExistingFile() {
			var items = Result;
			items.Each(item => Console.WriteLine(item.Name));
			Assert.AreEqual(1, items.Count);
			var child = items.First();
			Assert.AreEqual(Context.FileName, child.Name);
			Assert.AreEqual(Context.FilePathRelativeToRepositoryRoot, child.PathRelativeToRepositoryRoot);
		}

		public override IList<RepositoryItem> Act() {
			var controller = Context.Container.Get<ContentController>();
			var inputModel = new FileListInputModel {PhysicalApplicationPath = Context.AppRoot, Name = Context.REPO_NAME};
			return controller.GetFileList(inputModel).Items;
		}
	}

	[TestFixture]
	public class FileHierarchy : BaseQueryTest<RootFileAndFileInSubfolderContext, IList<RepositoryItem>> {

		[Test]
		public void HasTwoChildItems() {
			Assert.AreEqual(2, Result.Count);
		}

		[Test]
		public void FirstItemHasSameNameAndPathAsSubfolder() {
			var folderItem = Result.First();
			Assert.AreEqual(RootFileAndFileInSubfolderContext.FOLDER_NAME, folderItem.Name);
			Assert.AreEqual(@"\" + RootFileAndFileInSubfolderContext.FOLDER_NAME, folderItem.PathRelativeToRepositoryRoot);
		}

		[Test]
		public void GrandChildItemHasSameNameAndPathAsFileInSubfolder() {
			var grandchild = Result.First().Children.First();
			Assert.AreEqual(Context.OtherFileName, grandchild.Name);
			Assert.AreEqual(Context.OtherFilePathRelativeToRepositoryRoot, grandchild.PathRelativeToRepositoryRoot);
		}

		public override IList<RepositoryItem> Act() {
			var controller = Context.Container.Get<ContentController>();
			return controller.GetFileList(new FileListInputModel { PhysicalApplicationPath = Context.AppRoot, Name = Context.REPO_NAME}).Items;
		}
	}

	public class SingleFileContext : RepositoryFolderContext {
		public string FileName { get; set; }
		public string FilePath { get; private set; }
		public string FilePathRelativeToRepositoryRoot {
			get { return FilePath.Substring(RepositoryRoot.Length); }
		}
		public override void Create() {
			base.Create();
			FileName = Guid.NewGuid().ToString();
			FilePath = Path.Combine(RepositoryRoot, FileName);
			File.Create(FilePath).Dispose();
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
