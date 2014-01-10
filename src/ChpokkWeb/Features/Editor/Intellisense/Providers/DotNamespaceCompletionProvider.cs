using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public class DotNamespaceCompletionProvider {
		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel, int position) {
			if (token.ValueText == ".") {
				CommonSyntaxNode expression = null;
				var cSharpExpressionSyntax = token.Parent as Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax;
				if (cSharpExpressionSyntax != null) {
					expression = cSharpExpressionSyntax.Expression;
				}
				var vbNetExpressionSyntax = token.Parent as Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax;
				if (vbNetExpressionSyntax != null) {
					expression = vbNetExpressionSyntax.Expression;
				}
				if (expression != null) {
					var symbolInfo = semanticModel.GetSymbolInfo(expression);
					var symbol = symbolInfo.Symbol as INamespaceOrTypeSymbol;
					if (symbol != null) {
						var lookupSymbols = semanticModel.LookupSymbols(position, symbol);
						return from lookupSymbol in lookupSymbols select IntelOutputModel.IntelModelItem.FromSymbol(lookupSymbol);
					}
				}
			}
			return Enumerable.Empty<IntelOutputModel.IntelModelItem>();
		}
	}
}