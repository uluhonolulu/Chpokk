using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace SomeTests {
	[TestFixture]
	public class PassingTest {
		[Test]
		public void ImEmpty() {
		}

		[Test]
		public void SomeOutput() {
			Console.WriteLine("Output");
			Console.Error.WriteLine("Some error output");
		}
	}
}
