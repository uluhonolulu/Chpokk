using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Serenity.Jasmine;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class SerenityCheck {
		[Test]
		public void Test() {
			new JasmineCommand().Execute(new JasmineInput
			                             {Mode = JasmineMode.run, SerenityFile = @"F:\Projects\Fubu\Chpokk\src\serenity.txt"});
		}
	}
}
