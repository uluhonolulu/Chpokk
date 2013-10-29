using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.Editor.Intellisense;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class BaseCompletionTest {
		private readonly string _source;
		private readonly string _completionText;
		public BaseCompletionTest(string source, string completionText) {
			_source = source;
			_completionText = completionText;
		}

		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols() {
			var position = _source.IndexOf(_completionText) + _completionText.Length - 1;
			var mscorlibPath = typeof(String).Assembly.Location;
			return new CompletionProvider(new KeywordProvider()).GetSymbols(_source, position, new string[] { }, new[] { mscorlibPath }, LanguageNames.CSharp);

		} 
	}
}
