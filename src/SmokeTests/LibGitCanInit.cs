using System;
using System.IO;
using FubuCore;
using LibGit2Sharp;
using NUnit.Framework;

namespace SmokeTests {
	[TestFixture]
	public class LibGitCanInit {
		[Test]
		public void WithoutExceptions() {
			Console.WriteLine(Path.GetFullPath("."));
			var path = Path.GetFullPath(@"_PublishedWebsites\ChpokkWeb\bin\NativeBinaries\amd64");
			if (Directory.Exists(path)) {
				Console.WriteLine(path);
				Assert.IsNotEmpty(Directory.GetFiles(path, "git2*.dll"));				
			}

			
		}

		//[Test]
		//public void CanCloneFromAssembla() {
		//	var url = "https://rootgr1dorg@git.assembla.com/gr1dorg.git";
		//	var userName = "rootgr1dorg";
		//	var password = "5.25InchDisk ";
		//	url = "https://git.assembla.com/chpokk-private.git";//https://@git.assembla.com/.git - See more at: http://blog.assembla.com/assemblablog/tabid/12618/bid/70667/Git-over-HTTP.aspx#sthash.H34TZqNQ.dpuf
		//	userName = "uluhonolulu";
		//	//password = "xd11SvG23";
		//	//url = "https://uluhonolulu@bitbucket.org/uluhonolulu/chpokk-test.git";
		//	password = "p4SSw0RD";
		//	var targetFolder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ParentDirectory().AppendPath(@"Users\ulu\Repositories\private");
		//	Repository.Clone(url, targetFolder, credentials: new Credentials() { Username = userName, Password = password });
			
		//}
	}
}
