using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using SharpSvn;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class SvnSpike {
		[Test]
		public void CanCreateARepositoryAndCheckoutFilesAndCommit() {
			using (var client = new SharpSvn.SvnClient()) {
				SvnUpdateResult result;
				SvnRepositoryClient repositoryClient;
				var tmpFolder = Path.GetTempPath();
				Console.WriteLine(tmpFolder);
			}
		}
	}
}
