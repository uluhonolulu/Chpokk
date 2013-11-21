using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ICSharpCode.SharpDevelop;

namespace ChpokkWeb.Features.Editor.Intellisense {
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
	}
}