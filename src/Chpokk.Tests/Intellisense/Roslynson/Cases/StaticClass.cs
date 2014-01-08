using System.Linq;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	public class StaticClass: BaseCompletionTest {
		private const string STARTED_TYPING_CLASS_NAME_IN_METHOD = "using System; class ClassA { void method1(){Consol/**/} }";
		public StaticClass() : base(STARTED_TYPING_CLASS_NAME_IN_METHOD) { }

		[Test]
		public void ClassNameShouldAppearInReferences() {
			GetSymbols().Any(symbol => symbol.Name == "Console").ShouldBe(true);
		}
	}
}
