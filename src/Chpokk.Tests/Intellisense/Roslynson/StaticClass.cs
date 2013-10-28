using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class StaticClass: BaseCompletionTest {
		private const string STARTED_TYPING_CLASS_NAME_IN_METHOD = "using System; class ClassA { void method1(){Consol} }";
		public StaticClass() : base(STARTED_TYPING_CLASS_NAME_IN_METHOD, "Consol") { }

		[Test]
		public void ClassNameShouldAppearInReferences() {
			GetSymbols().Any(symbol => symbol.Name == "Console").ShouldBe(true);
		}
	}
}
