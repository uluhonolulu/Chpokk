using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Roslyn.Compilers;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	//provides the completion list of items not related to a particular class: parameter names, local vars, class and namespace names.
	public class GlobalCompletionProvider: ICompletionProvider {
		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel, int position) {
			if (!token.IsDot() && !token.IsMember()) {
				var lookupSymbols = semanticModel.LookupSymbols(position);
				return from lookupSymbol in lookupSymbols select IntelOutputModel.IntelModelItem.FromSymbol(lookupSymbol);
			}
			return Enumerable.Empty<IntelOutputModel.IntelModelItem>();
		}
	}
}