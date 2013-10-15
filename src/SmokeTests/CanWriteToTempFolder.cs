using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Framework;
using LibGit2Sharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;

namespace SmokeTests {
	[TestFixture]
	public class CanWriteToTempFolder {
		[Test]
		public void EmptyFile() {
			var path = Path.GetTempFileName();
			Console.WriteLine(path);
			File.WriteAllText(path, string.Empty);
			Console.WriteLine("Wrote a file");
			File.Delete(path);
		}

		[Test]
		public void CanCloneIntoIt() {
			var path = Path.Combine(Path.GetTempFileName().ParentDirectory(), "gr1dorg");
			try {
				Repository.Clone(sourceUrl: "https://git.assembla.com/gr1dorg.git", workdirPath: path,
			                 credentials: new Credentials() {Username = "rootgr1dorg", Password = "5.25InchDisk"});
				Directory.Move(path, Path.GetFullPath(@"_PublishedWebsites\ChpokkWeb\UserFiles\uluhonolulu_Twitter"));
			}
			finally {
				if(Directory.Exists(path))
					Directory.Delete(path, true);
			}
			
				//RepoUrl=http%3A%2F%2Fgit.assembla.com%2Fgr1dorg.git&Username=rootgr1dorg&Password=5.25InchDisk
		}
	}
}
