using System.Linq;
using MbUnit.Framework;
using Roslyn.Compilers;
using Shouldly;

namespace UnitTests.Roslynson.Cases {
	public class ThisDotMember:BaseCompletionTest {
		private const string TYPED_THIS_DOT_IN_A_METHOD = "class ClassA { string field1; void method1(){this./**/} }";

		public ThisDotMember() : base(TYPED_THIS_DOT_IN_A_METHOD) { }

		[Test]
		public void HasMemberNamesInIntellisenseData() {
			var symbols = GetSymbols(LanguageNames.CSharp);
			symbols.Any(symbol => symbol.Name == "field1").ShouldBe(true);
			symbols.Any(symbol => symbol.Name == "ClassA").ShouldBe(false);
		}


	}
}
