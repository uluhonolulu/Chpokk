﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FubuCore;
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
			var targetFolder = AppDomain.CurrentDomain.SetupInformation.ApplicationBase.ParentDirectory().AppendPath(@"Users\ulu\Repositories\private");
			var cloneOptions = new CloneOptions
				{
					CredentialsProvider = (s, fromUrl, types) => new UsernamePasswordCredentials() {Username = userName, Password = password}
				};
			Repository.Clone(url, targetFolder, cloneOptions);
		}

		[Test]
		public void CanCloneFromBitbucket() {
			var url = "https://uluhonolulu@bitbucket.org/uluhonolulu/chpokk-test.git";
			var userName = "uluhonolulu";
			var password = "xd11SvG23";
			var targetFolder = @"D:\Projects\Chpokk\src\ChpokkWeb\UserFiles\ulu\private";
			if (Directory.Exists(targetFolder)) {
				Directory.Delete(targetFolder);
			}
			var cloneOptions = new CloneOptions {
				CredentialsProvider = (s, fromUrl, types) => new UsernamePasswordCredentials() { Username = userName, Password = password }
			};
			try {
				Repository.Clone(url, targetFolder, cloneOptions);
			}
			finally {
				if (Directory.Exists(targetFolder)) {
					Directory.Delete(targetFolder);
				}	
			}
			
		}
	}
}
