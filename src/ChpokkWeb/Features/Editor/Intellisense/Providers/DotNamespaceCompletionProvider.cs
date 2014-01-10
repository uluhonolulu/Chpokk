using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public class DotNamespaceCompletionProvider {
		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel, int position) {
			if (token.ValueText == ".") {
				var cSharpExpressionSyntax = token.Parent as Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax;
				if (cSharpExpressionSyntax != null) {
					var symbolInfo = semanticModel.GetSymbolInfo(cSharpExpressionSyntax.Expression);
					var symbol = symbolInfo.Symbol;
					var lookupSymbols = semanticModel.LookupSymbols(position, symbol as INamespaceOrTypeSymbol);
					return from lookupSymbol in lookupSymbols select IntelOutputModel.IntelModelItem.FromSymbol(lookupSymbol);
				}
				var vbNetExpressionSyntax = token.Parent as Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax;
				if (vbNetExpressionSyntax != null) {
					var symbolInfo = semanticModel.GetSymbolInfo(vbNetExpressionSyntax.Expression);
					var symbol = symbolInfo.Symbol;
					var lookupSymbols = semanticModel.LookupSymbols(position, symbol as INamespaceOrTypeSymbol);
					return from lookupSymbol in lookupSymbols select IntelOutputModel.IntelModelItem.FromSymbol(lookupSymbol);
				}
			}
			return Enumerable.Empty<IntelOutputModel.IntelModelItem>();
		}
	}
}