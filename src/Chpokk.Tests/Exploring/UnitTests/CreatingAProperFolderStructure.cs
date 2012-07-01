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
			var item = result.Single();

			Assert.AreEqual("Class1", item.Name);
			Assert.AreEqual(@"root\Class1", item.PathRelativeToRepositoryRoot);
			Assert.AreEqual("file", item.Type);
		}

		[Test]
		public void FileInASubfolderReturnsSubfolderAndFileAsAChild() {
			var source = new[] {new FileItem {Path = @"Subfolder\\Class1"}};
			var result = _converter.Convert(source, "root");
			Assert.AreEqual(1, result.Count());
			//var item = result.Single();

			//Assert.AreEqual("Class1", item.Name);
			//Assert.AreEqual(@"root\Class1", item.PathRelativeToRepositoryRoot);
			//Assert.AreEqual("file", item.Type);
			
		}
	}
}
