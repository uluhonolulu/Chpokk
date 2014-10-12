using MbUnit.Framework;
using Roslyn.Compilers;
using Shouldly;

namespace UnitTests.Roslynson.Cases {
	[TestFixture]
	public class InAMember:BaseCompletionTest {
		public InAMember() : base("using System; class ClassA { string field1; void method1(string param1){ string var1;/**/ } }") { }

		[Test]
		public void ShouldHaveClassNames() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "Console");
		}

		[Test]
		public void ShouldHaveNamespaceNames() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "System");
		}

		[Test]
		public void ShouldHaveMemberNames() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "field1");
		}


		[Test]
		public void ShouldHaveParameterNames() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "param1");
		}

		[Test]
		public void ShouldHaveVariableNames() {
			GetSymbols(LanguageNames.CSharp).ShouldContain(item => item.Name == "var1");
		}
	}
}
