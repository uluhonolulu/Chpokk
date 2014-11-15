using System;
using System.Collections.Generic;
using System.Text;
using Arractas;
using Chpokk.Tests.Infrastructure;
using Emkay.S3;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using System.Linq;

namespace Chpokk.Tests.Amazon {
	[TestFixture]
	public class ListWebConfigs: BaseQueryTest<SimpleConfiguredContext, IEnumerable<string>> {
		[Test]
		public void ShowMe() {
			foreach (var path in Result) {
				Console.WriteLine(path);
			}
		}

		public override IEnumerable<string> Act() {
			var client = Context.Container.Get<IS3Client>();
			return client.EnumerateChildren("chpokk").Where(s => s.EndsWith("vstemplate"));
		}
	}
}
