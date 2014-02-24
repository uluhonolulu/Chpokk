using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class IvonnaDir {
		[Test]
		public void Test() {
			Console.WriteLine(Environment.CurrentDirectory);
			Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
			//var testSession = new TestSession();
		}
	}
}
