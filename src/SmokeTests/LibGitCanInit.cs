using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

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
