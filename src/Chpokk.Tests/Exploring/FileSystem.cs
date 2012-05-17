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

namespace Chpokk.Tests.Exploring {
	[TestFixture]
	public class FileSystem : BaseQueryTest<SingleFileContext, HtmlTag> {
		[Test]
		public void ReturnsAnUlTag() {
			Assert.AreEqual("ul", Result.TagName());
		}

		[Test]
		public void ContainsTheFileName() {
			var child = Result.FirstChild();
			Assert.AreEqual(Context.FileName, child.Data("name"));
			Assert.AreEqual(Context.FilePathRelativeToRepositoryRoot, child.Data("path"));
			Assert.AreEqual(Context.FileName, child.Text());
		}

		public override HtmlTag Act() {
			var controller = Context.Container.Get<ContentController>();
			return controller.GetFileList(new FileListModel(){Name = "stuff", PhysicalApplicationPath = Path.GetFullPath("..")});
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
			if (File.Exists(FilePath)) {
				File.Delete(FilePath);
			}
		}
	}
}
