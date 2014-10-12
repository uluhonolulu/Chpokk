using System.Collections.Generic;
using System.Linq;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public class DotCompletionProvider : ICompletionProvider {
		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel, int position) {
			if (token.IsDot() || token.IsMember()) {
				var masterNode = GetMasterNode(token); //the node before the dot
				//TODO: masterNode can be PredefinedTypeSymbol
				if (masterNode != null) {
					var symbolInfo = semanticModel.GetSymbolInfo(masterNode);
					var symbol = symbolInfo.Symbol as INamespaceOrTypeSymbol; //if the master node is a namespace or a class name
					if (symbol == null) {
						symbol = semanticModel.GetTypeInfo(masterNode).Type; //if the master node is an instance of a class
					}
					if (symbol != null) {
						var lookupSymbols = semanticModel.LookupSymbols(position, symbol);
						return from lookupSymbol in lookupSymbols select IntelOutputModel.IntelModelItem.FromSymbol(lookupSymbol);
					}
				}
			}
			return Enumerable.Empty<IntelOutputModel.IntelModelItem>();
		}

		private CommonSyntaxNode GetMasterNode(CommonSyntaxToken token) {
			CommonSyntaxNode expression = null;
			//member access
			var sharpExpressionSyntax = token.Parent.AncestorsAndSelf().OfType<Roslyn.Compilers.CSharp.MemberAccessExpressionSyntax>().FirstOrDefault();
			if (sharpExpressionSyntax != null)
				expression = sharpExpressionSyntax.Expression;
			var vbNetExpressionSyntax = token.Parent.AncestorsAndSelf().OfType<Roslyn.Compilers.VisualBasic.MemberAccessExpressionSyntax>().FirstOrDefault();
			if (vbNetExpressionSyntax != null)
				expression = vbNetExpressionSyntax.Expression;
			//namespace
			var sharpQualifiedNameSyntax = token.Parent as Roslyn.Compilers.CSharp.QualifiedNameSyntax;
			if (sharpQualifiedNameSyntax != null)
				expression = sharpQualifiedNameSyntax.Left;
			var vbNetQualifiedNameSyntax = token.Parent as Roslyn.Compilers.VisualBasic.QualifiedNameSyntax;
			if (vbNetQualifiedNameSyntax != null)
				expression = vbNetQualifiedNameSyntax.Left;
			return expression;
		}
	}
}