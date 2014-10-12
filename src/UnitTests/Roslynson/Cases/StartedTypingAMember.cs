using MbUnit.Framework;
using Roslyn.Compilers;
using Shouldly;

namespace UnitTests.Roslynson.Cases {
	[TestFixture]
	public class StartedTypingAMember: BaseCompletionTest {
		public StartedTypingAMember() : base("using System; class ClassA { void method1(){Console.w/**/} }") { }

		[Test]
		public void ShouldContainOnlyMembersOfTheClass() {
			var symbols = GetSymbols(LanguageNames.CSharp);
			symbols.ShouldContain(item => item.Name == "WriteLine");
			symbols.ShouldNotContain(item => item.Name == "class");
			symbols.ShouldNotContain(item => item.Name == "System");
		}
	}
}
