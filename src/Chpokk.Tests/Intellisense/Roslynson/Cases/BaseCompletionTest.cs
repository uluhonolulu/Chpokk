using System;
using System.Collections.Generic;
using ChpokkWeb.Features.Editor.Intellisense;
using ChpokkWeb.Features.Editor.Intellisense.Providers;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	public class BaseCompletionTest {
		private readonly string _source;
		public BaseCompletionTest(string source) {
			_source = source;
		}

		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(string language) {
			var position = _source.IndexOf("/**/") - 1;
			var mscorlibPath = typeof(String).Assembly.Location;
			return new CompletionProvider(new KeywordProvider()).GetSymbols(_source.Replace("/**/", ""), position, new string[] { }, new[] { mscorlibPath }, language);

		} 

		public CommonSyntaxToken GetToken(string language = LanguageNames.CSharp) {
			CommonSyntaxTree tree = (language == LanguageNames.CSharp)
				                        ? (CommonSyntaxTree) Roslyn.Compilers.CSharp.SyntaxTree.ParseText(_source)
				                        : Roslyn.Compilers.VisualBasic.SyntaxTree.ParseText(_source);
			var position = _source.IndexOf("/**/") - 1;
			return tree.GetRoot().FindToken(position);
		}
	}
}
