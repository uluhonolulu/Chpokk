using System;
using System.Collections.Generic;
using System.Text;
using FubuTestingSupport;
using Gallio.Framework;
using LibGit2Sharp;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests {
	[TestFixture]
	public class SpikeCheckout {
		[Test]
		public void Test() {
			using (var repo = new Repository(@"F:\Projects\Fubu\Chpokk\ChpokkWeb\Repa")) {
				repo.Remotes["origin"].ShouldBeNull();
			}
		}
	}
}
