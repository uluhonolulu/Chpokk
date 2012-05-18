using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Features.Repa;
using Gallio.Framework;
using HtmlTags;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class FileSystem : BaseQueryTest<SingleFileContext, IList<RepositoryItem>> {

		[Test]
		public void HasOneRootItem() {
			Assert.AreEqual(1, Result.Count);
			//Assert.AreSame(new RepositoryItem{}, Result.First());
			//var child = Result.First();
			//Assert.AreEqual(Context.FileName, child.Data("name"));
			//Assert.AreEqual(Context.FilePathRelativeToRepositoryRoot, child.Data("path"));
			//Assert.AreEqual(Context.FileName, child.Text());
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
			return controller.GetFileList(new FileListModel {PhysicalApplicationPath = Path.GetFullPath("..")});
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
			foreach (var file in Directory.GetFiles(RepositoryRoot)) {
				File.Delete(file);
			}
			//if (File.Exists(FilePath)) {
			//    File.Delete(FilePath);
			//}
		}
	}
}
