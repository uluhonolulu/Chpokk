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

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class SearchingForNuSpec: BaseCommandTest<SimpleConfiguredContext> {
		[Test]
		public void Test() {
			//
			// TODO: Add test logic here
			//
		}

		public override void Act() {
			var client = Context.Container.Get<IS3Client>();
			var files = from file in client.EnumerateChildren("chpokk") where file.EndsWith("dll") && file.Contains("/packages/") select file;
			foreach (var file in files) {
				Console.WriteLine(file);
			}
		}
	}
}
