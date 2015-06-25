using System;
using System.Collections.Generic;
using System.Text;
using ChpokkWeb.Features.Editor.Intellisense.Providers;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;

namespace Chpokk.Tests.Spikes {
	[TestFixture]
	public class GetVbKeywords {
		[Test]
		public void Test() {
			var list = new KeywordProvider().VBNetKeywords.Join("|");
			Console.WriteLine(list);
		}
	}
}
