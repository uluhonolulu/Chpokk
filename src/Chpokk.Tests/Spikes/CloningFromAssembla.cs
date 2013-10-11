using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using LibGit2Sharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class CloningFromAssembla {
		[Test]
		public void CanCloneSuccessfully() {
			var url = "https://rootgr1dorg@git.assembla.com/gr1dorg.git";
			var userName = "rootgr1dorg";
			var password = "5.25InchDisk ";
			url = "https://git.assembla.com/chpokk-private.git";//https://@git.assembla.com/.git - See more at: http://blog.assembla.com/assemblablog/tabid/12618/bid/70667/Git-over-HTTP.aspx#sthash.H34TZqNQ.dpuf
			userName = "uluhonolulu";
			//password = "xd11SvG23";
			//url = "https://uluhonolulu@bitbucket.org/uluhonolulu/chpokk-test.git";
			password = "p4SSw0RD";
			var targetFolder = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\private9";
			Repository.Clone(url, targetFolder, credentials: new Credentials(){Username = userName, Password = password});
		}

		[Test]
		public void CanCloneFromBitbucket() {
			var url = "https://uluhonolulu@bitbucket.org/uluhonolulu/chpokk-test.git";
			var userName = "uluhonolulu";
			var password = "xd11SvG23";
			var targetFolder = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\private5";
			Repository.Clone(url, targetFolder, credentials: new Credentials(){Username = userName, Password = password});
			
		}
	}
}
