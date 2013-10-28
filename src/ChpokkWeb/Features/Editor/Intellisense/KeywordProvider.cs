using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using ICSharpCode.SharpDevelop;
using Roslyn.Compilers.CSharp;

namespace ChpokkWeb.Features.Editor.Intellisense {
	public class KeywordProvider {
		public KeywordProvider() {
			var memberInfos = typeof(SyntaxKind).GetMembers(BindingFlags.Public | BindingFlags.Static);
			var keywords = from memberInfo in memberInfos
						   where memberInfo.Name.EndsWith("Keyword")
						   orderby memberInfo.Name
						   select memberInfo.Name.CutoffEnd("Keyword").ToLower();
			Keywords = keywords.OrderBy(s => s).ToArray();
		}

		public IEnumerable<string> Keywords { get; private set; }
	}
}