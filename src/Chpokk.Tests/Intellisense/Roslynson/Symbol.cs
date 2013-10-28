using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Roslyn.Compilers;
using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class Symbol {
		private readonly CompletionProvider _completionProvider = new CompletionProvider();

		[Test]
		public void HasLocalSymbolsInIntellisenseData() {
			var source = "class ClassA { string field1; void method1(){fie} }";
			var position = source.IndexOf("fie") + "fie".Length - 1;
			var mscorlibPath = typeof (String).Assembly.Location;
			var symbols = new CompletionProvider().GetSymbols(source, position, new string[] { }, new string[] { mscorlibPath }, LanguageNames.CSharp);

			var result = from symbol in symbols where symbol.Name.StartsWith("fie") select symbol;

			result.Any(symbol => symbol.Name == "field1").ShouldBe(true);

		}
	}
}
