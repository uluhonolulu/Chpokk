using System;
using System.IO;
using NUnit.Framework;

namespace SmokeTests {
	[TestFixture]
	public class LibGitCanInit {
		[Test]
		public void WithoutExceptions() {
			Console.WriteLine(Path.GetFullPath("."));
			var path = Path.GetFullPath(@"_PublishedWebsites\ChpokkWeb\bin\NativeBinaries\amd64");
			Console.WriteLine(path);
			Assert.IsNotEmpty(Directory.GetFiles(path, "git2*.dll"));
			
		}
	}
}
