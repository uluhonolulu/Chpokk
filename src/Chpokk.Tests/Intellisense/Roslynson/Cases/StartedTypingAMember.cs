using System;
using System.Collections.Generic;
using System.Text;
using Gallio.Framework;
using MbUnit.Framework;
using MbUnit.Framework.ContractVerifiers;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	[TestFixture]
	public class StartedTypingAMember: BaseCompletionTest {
		public StartedTypingAMember() : base("using System; class ClassA { void method1(){Console.w/**/} }") { }

		[Test]
		public void ShouldContainOnlyMembersOfTheClass() {
			var symbols = GetSymbols();
			symbols.ShouldContain(item => item.Name == "WriteLine");
			symbols.ShouldNotContain(item => item.Name == "class");
			symbols.ShouldNotContain(item => item.Name == "System");
		}
	}
}
