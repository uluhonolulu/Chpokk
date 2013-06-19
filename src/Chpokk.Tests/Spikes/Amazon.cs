using System;
using System.Collections.Generic;
using System.Text;
using Emkay.S3;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class Amazon {
		[Test]
		public void EnumeratingTheBuckets() {
			var factory = new S3ClientFactory();
			var client = factory.Create("AKIAIHOC7V5PPD4KIZBQ", "UJlRXeixN8/cQ5XuZK9USGUMzhnxsGs7YYiZpozM");
			client.EnumerateBuckets().Each(str => Console.WriteLine(str));
			client.EnumerateChildren("chpokk").Each(str => Console.WriteLine(str));
		}
	}
}
