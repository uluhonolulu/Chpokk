using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class ThisDotMember:BaseCompletionTest {
		private const string TYPED_THIS_DOT_IN_A_METHOD = "class ClassA { string field1; void method1(){this.} }";

		public ThisDotMember() : base(TYPED_THIS_DOT_IN_A_METHOD, "this.") { }

		[Test]
		public void HasMemberNamesInIntellisenseData() {
			var symbols = GetSymbols();
			symbols.Any(symbol => symbol.Name == "field1").ShouldBe(true);
			symbols.Any(symbol => symbol.Name == "ClassA").ShouldBe(false);
		}


	}
}
