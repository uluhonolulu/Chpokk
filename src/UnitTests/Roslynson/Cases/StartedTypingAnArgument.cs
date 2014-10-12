using ChpokkWeb.Features.Editor.Intellisense.Providers;
using MbUnit.Framework;
using Roslyn.Compilers;
using Shouldly;

namespace UnitTests.Roslynson.Cases {
	[TestFixture]
	public class StartedTypingAnArgument : BaseCompletionTest {
		public StartedTypingAnArgument() : base("using System; class ClassA { void method1(){Double.Parse(n, /**/)} }") { }

		[Test]
		public void TokenShouldNotbeMemberOrDot() {
			var token = GetToken();
			token.IsMember().ShouldBe(false);
		}

		[Test]
		public void ShouldDisplayGlobalSymbols() {
			var symbols = GetSymbols(LanguageNames.CSharp);
			symbols.ShouldContain(item => item.Name == "System");
			//symbols.ShouldContain(item => item.Name == "class");
		}
	}
}
