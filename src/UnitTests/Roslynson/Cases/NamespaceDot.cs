using MbUnit.Framework;
using Roslyn.Compilers;
using Shouldly;

namespace UnitTests.Roslynson.Cases {
	public class NamespaceDot: BaseCompletionTest {
		public NamespaceDot() : base("class ClassA{ void method(){ System./**/ } }") {}

		[Test]
		public void CompletesNestedNamespace() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "Collections");
		}

		[Test]
		public void CompletesClassName() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "String");
		}
	}
}
