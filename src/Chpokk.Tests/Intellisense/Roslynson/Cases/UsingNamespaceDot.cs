using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Roslyn.Compilers;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	[TestFixture]
	public class UsingNamespaceDot: BaseCompletionTest {
		public UsingNamespaceDot() : base("using System./**/") {}


		[Test]
		public void CompletesNestedNamespace() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "Collections");
		}

	}
}
