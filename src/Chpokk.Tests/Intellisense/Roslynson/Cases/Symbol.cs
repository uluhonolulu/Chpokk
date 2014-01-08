using System.Linq;
using MbUnit.Framework;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	public class Symbol:BaseCompletionTest {
		private const string STARTED_TYPING_FIELD_NAME = "class ClassA { string field1; void method1(string field2){fie/**/} }";

		public Symbol() : base(STARTED_TYPING_FIELD_NAME) { }

		[Test]
		public void HasLocalSymbolsInIntellisenseData() {
			var symbols = GetSymbols();
			var result = from symbol in symbols where symbol.Name.StartsWith("fie") select symbol;

			result.Any(symbol => symbol.Name == "field1").ShouldBe(true);

		}

	}
}
