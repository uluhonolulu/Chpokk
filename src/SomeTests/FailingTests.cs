using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;

namespace SomeTests {
	[TestFixture]
	public class FailingTests {
		[Test]
		public void JustFails() {
			Assert.Fail("Just fails!!!");
		}

		[Test]
		public void Throws() {
			throw new Exception("OMG");
		}
	}
}
