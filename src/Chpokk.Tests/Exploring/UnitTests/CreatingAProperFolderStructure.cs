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

			var fileItem = folderItem.Children.Single();
			Assert.AreEqual("Class1", fileItem.Name);
			Assert.AreEqual(@"root\Subfolder\Class1", fileItem.PathRelativeToRepositoryRoot);
		}

		[Test]
		public void TwoFilesInTwoSubfolders() {
			var source = new[] {new FileItem {Path = @"Subfolder1\Class1"}, new FileItem {Path = @"Subfolder2\Class2"}};
			var result = _converter.Convert(source, "root");
			Assert.AreEqual(2, result.Count());

			var folderItem1 = result.First();
			folderItem1.Children.Each(item => Console.WriteLine(item.Name));
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

		[Test]
		public void SubSubFolder() {
			var source = new[] {new FileItem {Path = @"Subfolder1\Subfolder2\Class"}};
			var result = _converter.Convert(source, "root");
			Assert.AreEqual(1, result.Count());
			var folderItem = result.Single();

			Assert.AreEqual("Subfolder1", folderItem.Name);
			Assert.AreEqual(@"root\Subfolder1", folderItem.PathRelativeToRepositoryRoot);
			Assert.AreEqual("folder", folderItem.Type);
			Assert.AreEqual(1, folderItem.Children.Count);

			var subFolderItem = folderItem.Children.Single();
			Assert.AreEqual("Subfolder2", subFolderItem.Name);
			Assert.AreEqual("folder", subFolderItem.Type);
			Assert.AreEqual(1, subFolderItem.Children.Count);


			var fileItem = subFolderItem.Children.Single();
			Assert.AreEqual("Class", fileItem.Name);
			Assert.AreEqual(@"root\Subfolder1\Subfolder2\Class", fileItem.PathRelativeToRepositoryRoot);
			Assert.AreEqual("file", fileItem.Type);
		}
	}
}
