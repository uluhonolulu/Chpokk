using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using Ivonna.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using TypeMock.ArrangeActAssert;

namespace Chpokk.Tests.Spikes {
	[TestFixture, RunOnWeb]
	public class IvonnaDir {
		[Test]
		public void Test() {
			Console.WriteLine(Environment.CurrentDirectory);
			Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
			//var testSession = new TestSession();
		}

		[Test, Isolated]
		public void ReplaceDateTimeNowWithValue() {
			Isolate.WhenCalled(() => DateTime.Now).WillReturn(new DateTime(2010, 1, 2));

			Assert.AreEqual(new DateTime(2010, 1, 2), DateTime.Now);
		}
	}
}
