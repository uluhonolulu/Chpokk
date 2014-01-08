using System;
using System.Linq;
using System.Reflection;
using MbUnit.Framework;
using Roslyn.Compilers.CSharp;
using ICSharpCode.SharpDevelop;

namespace Chpokk.Tests.Intellisense.Roslynson.Cases {
	public class Keywords {
		[Test]
		public void CanGetAllKeywords() {
			var memberInfos = typeof (SyntaxKind).GetMembers(BindingFlags.Public | BindingFlags.Static);
			var keywords = from memberInfo in memberInfos
			                 where memberInfo.Name.EndsWith("Keyword") 
							 orderby memberInfo.Name
			                 select memberInfo.Name.CutoffEnd("Keyword").ToLower();
			var directives = from memberInfo in memberInfos
							 where memberInfo.Name.EndsWith("Directive")
							 select memberInfo.Name.CutoffEnd("Directive").ToLower();

			foreach (var directive in directives) {
				Console.WriteLine(directive);
			}
			Console.WriteLine();
			foreach (var keyword in keywords) {
				Console.WriteLine(keyword);
			}
		}
	}
}
