using System;
using System.Collections.Generic;
using ChpokkWeb.Features.Editor.Intellisense;
using Roslyn.Compilers;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	public class BaseCompletionTest {
		private readonly string _source;
		public BaseCompletionTest(string source) {
			_source = source;
		}

		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols() {
			var position = _source.IndexOf("/**/") - 1;
			var mscorlibPath = typeof(String).Assembly.Location;
			return new CompletionProvider(new KeywordProvider()).GetSymbols(_source, position, new string[] { }, new[] { mscorlibPath }, LanguageNames.CSharp);

		} 
	}
}
