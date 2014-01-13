using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roslyn.Compilers;
//using Roslyn.Compilers.CSharp;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public class DotlessCompletionProvider: ICompletionProvider {
		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel, int position) {
			if (!token.IsDot()) {
				var typeSymbol = GetContainingClass(token, semanticModel);
				var lookupSymbols = semanticModel.LookupSymbols(position, typeSymbol);
				return from lookupSymbol in lookupSymbols select IntelOutputModel.IntelModelItem.FromSymbol(lookupSymbol);
			}
			return Enumerable.Empty<IntelOutputModel.IntelModelItem>();
		}

		private static ITypeSymbol GetContainingClass(CommonSyntaxToken token, ISemanticModel semanticModel) {
			var syntaxNodes = token.Parent.AncestorsAndSelf();
			var classDeclarationSyntax = syntaxNodes.OfType<Roslyn.Compilers.CSharp.ClassDeclarationSyntax>().Cast<CommonSyntaxNode>()
				.Union(syntaxNodes.OfType<Roslyn.Compilers.VisualBasic.ClassBlockSyntax>())
				.FirstOrDefault();
			if (classDeclarationSyntax != null) {
				var typeInfo = semanticModel.GetTypeInfo(classDeclarationSyntax);
				var symbol = semanticModel.GetDeclaredSymbol(classDeclarationSyntax);
				return symbol as INamedTypeSymbol;
			}
			return null;
		}
	}
}