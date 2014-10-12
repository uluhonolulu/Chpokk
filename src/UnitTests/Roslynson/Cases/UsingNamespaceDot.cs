using MbUnit.Framework;
using Roslyn.Compilers;
using Shouldly;

namespace UnitTests.Roslynson.Cases {
	[TestFixture]
	public class UsingNamespaceDot: BaseCompletionTest {
		public UsingNamespaceDot() : base("using System./**/") {}


		[Test]
		public void CompletesNestedNamespace() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "Collections");
		}

	}
}
