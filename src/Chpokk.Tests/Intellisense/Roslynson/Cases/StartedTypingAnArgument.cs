using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Roslyn.Compilers;
using Shouldly;
using ChpokkWeb.Features.Editor.Intellisense.Providers;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
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
