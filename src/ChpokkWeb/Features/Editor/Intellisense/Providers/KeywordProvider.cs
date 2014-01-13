using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ICSharpCode.SharpDevelop;
using Roslyn.Compilers.Common;

namespace ChpokkWeb.Features.Editor.Intellisense.Providers {
	public class KeywordProvider {
		public KeywordProvider() {
			//C#
			var memberInfos = typeof(Roslyn.Compilers.CSharp.SyntaxKind).GetMembers(BindingFlags.Public | BindingFlags.Static);
			var keywords = from memberInfo in memberInfos
						   where memberInfo.Name.EndsWith("Keyword")
						   orderby memberInfo.Name
						   select memberInfo.Name.CutoffEnd("Keyword").ToLower();
			CSharpKeywords = keywords.OrderBy(s => s).ToArray();

			memberInfos = typeof(Roslyn.Compilers.VisualBasic.SyntaxKind).GetMembers(BindingFlags.Public | BindingFlags.Static);
			keywords = from memberInfo in memberInfos
					   where memberInfo.Name.EndsWith("Keyword")
					   orderby memberInfo.Name
					   select memberInfo.Name.CutoffEnd("Keyword");
			VBNetKeywords = keywords.OrderBy(s => s).ToArray();
		}

		public IEnumerable<string> CSharpKeywords { get; private set; }
		public IEnumerable<string> VBNetKeywords { get; private set; }

		public IEnumerable<IntelOutputModel.IntelModelItem> GetSymbols(CommonSyntaxToken token, ISemanticModel semanticModel,
		                                                               int position) {
			if (!token.IsDot()) {
				if (semanticModel.IsCSharpModel()) {
					return from keyword in CSharpKeywords select IntelOutputModel.IntelModelItem.FromKeyword(keyword);
				}
				else {
					return from keyword in VBNetKeywords select IntelOutputModel.IntelModelItem.FromKeyword(keyword);
				}
			}
			return Enumerable.Empty<IntelOutputModel.IntelModelItem>();
		}
	}
}