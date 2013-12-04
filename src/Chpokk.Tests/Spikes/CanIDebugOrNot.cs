using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Gallio.Framework;
using Ivonna.Core;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using TypeMock;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class CanIDebugOrNot {
		[Test]
		public void Test() {
			TestMother.GetInstance(Path.GetFullPath(".."), "/", false, null);
		}
	}
}
