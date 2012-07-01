using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChpokkWeb.Features.Exploring;
using MbUnit.Framework;

namespace Chpokk.Tests.Exploring.UnitTests {
	public class CreatingAProperFolderStructure {
		private readonly FileItemToProjectItemConverter _converter = new FileItemToProjectItemConverter();

		[Test]
		public void RootFileReturnsSingleFileItem() {
			var source = new[] {new FileItem {Path = "Class1"}};
			var result = _converter.Convert(source, "root");
			Assert.AreEqual(1, result.Count());
			var item = result.Single();

			Assert.AreEqual("Class1", item.Name);
			Assert.AreEqual(@"root\Class1", item.PathRelativeToRepositoryRoot);
			Assert.AreEqual("file", item.Type);
		}

		[Test]
		public void FileInASubfolderReturnsSubfolderAndFileAsAChild() {
			var source = new[] {new FileItem {Path = @"Subfolder\Class1"}};
			var result = _converter.Convert(source, "root");
			Assert.AreEqual(1, result.Count());
			var folderItem = result.Single();

			Assert.AreEqual("Subfolder", folderItem.Name);
			Assert.AreEqual(@"root\Subfolder", folderItem.PathRelativeToRepositoryRoot);
			Assert.AreEqual("folder", folderItem.Type);
			Assert.AreEqual(1, folderItem.Children.Count);

		}

		[Test]
		public void TwoFilesInTwoSubfolders() {
			var source = new[] {new FileItem {Path = @"Subfolder1\Class1"}, new FileItem {Path = @"Subfolder2\Class2"}};
			var result = _converter.Convert(source, "root");
			Assert.AreEqual(2, result.Count());

			var folderItem1 = result.First();
			Assert.AreEqual(1, folderItem1.Children.Count);
		}

		[Test]
		public void TwoFilesInSameSubfolder() {
			var source = new[] {new FileItem {Path = @"Subfolder\Class1"}, new FileItem {Path = @"Subfolder\Class2"}};
			var result = _converter.Convert(source, "root");
			Assert.AreEqual(1, result.Count());

			var folderItem = result.First();
			Assert.AreEqual(2, folderItem.Children.Count);			
		}
	}
}
