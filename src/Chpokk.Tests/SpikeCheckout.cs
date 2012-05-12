using LibGit2Sharp;
using NUnit.Framework;

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
