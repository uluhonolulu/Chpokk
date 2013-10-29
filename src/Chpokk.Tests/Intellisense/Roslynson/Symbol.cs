using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MbUnit.Framework;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class Symbol:BaseCompletionTest {
		private const string STARTED_TYPING_FIELD_NAME = "class ClassA { string field1; void method1(){fie} }";

		public Symbol() : base(STARTED_TYPING_FIELD_NAME, "fie") { }

		[Test]
		public void HasLocalSymbolsInIntellisenseData() {
			var symbols = GetSymbols();
			var result = from symbol in symbols where symbol.Name.StartsWith("fie") select symbol;

			result.Any(symbol => symbol.Name == "field1").ShouldBe(true);

		}
	}
}
