using System.Collections.Generic;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public interface ICompletionProvider {
		IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel, int position);
	}
}