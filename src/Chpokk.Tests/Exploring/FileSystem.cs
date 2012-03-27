using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using ChpokkWeb.Repa;
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
			Assert.AreEqual(child.Data("name"), Context.FileName);
			Assert.AreEqual(child.Data("path"), Context.FilePath);
		}

		public override HtmlTag Act() {
			var controller = Context.Container.Get<ContentController>();
			return controller.GetFileList(new RepositoryFileContentModel(){Name = "stuff"});
		}
	}

	public class SingleFileContext : SimpleConfiguredContext, IDisposable {
		public string FileName { get; set; }
		public string FilePath { get; private set; }
		public override void Create() {
			base.Create();
			var folder = Path.Combine(Path.GetFullPath(@".."), RepositoryInfo.Path);
			if (!Directory.Exists(folder))
				Directory.CreateDirectory(folder);
			FileName = Guid.NewGuid().ToString();
			FilePath = Path.Combine(folder, FileName);
			File.Create(FilePath).Dispose();
		}

		public void Dispose() {
			if (File.Exists(FilePath)) {
				File.Delete(FilePath);
			}
		}
	}
}
