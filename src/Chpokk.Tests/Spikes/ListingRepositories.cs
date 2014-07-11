using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using Emkay.S3;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using FubuCore;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class ListingRepositories: BaseQueryTest<SimpleConfiguredContext, IEnumerable<string>> {
		[Test]
		public void LetsSeeTheList() {
			foreach (var path in Result) {
				Console.WriteLine(path);
			}
		}

		public override IEnumerable<string> Act() {
			var client = Context.Container.Get<IS3Client>();
			var prefix = "UserFiles/amitshankar123_Google/Repositories/";
			var fullRemotePaths = client.EnumerateChildren("chpokk", prefix);
			return (from path in fullRemotePaths select path.Substring(prefix.Length).Split('/')[0]).Distinct();
		}
	}
}
