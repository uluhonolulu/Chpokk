using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public class DotNamespaceCompletionProvider {
		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel, int position) {
			if (token.ValueText == ".") {
				var expressionSyntax = token.Parent as Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax;
				if (expressionSyntax != null) {
					var symbolInfo = semanticModel.GetSymbolInfo(expressionSyntax.Expression);
					var symbol = symbolInfo.Symbol;
					var lookupSymbols = semanticModel.LookupSymbols(position, symbol as INamespaceOrTypeSymbol);
					return from lookupSymbol in lookupSymbols select IntelOutputModel.IntelModelItem.FromSymbol(lookupSymbol);
				}
			}
			return Enumerable.Empty<IntelOutputModel.IntelModelItem>();
		}
	}
}