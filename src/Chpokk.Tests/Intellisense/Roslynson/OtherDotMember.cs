using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChpokkWeb.Features.Editor.Intellisense;
using MbUnit.Framework;
using Roslyn.Compilers;
using Shouldly;

namespace Chpokk.Tests.Intellisense.Roslynson {
	public class OtherDotMember {
		[Test]
		public void HasMemberNamesOfAnotherClassInIntellisenseData() {
			var source = "class ClassA { string field1; void method1(){new ClassB().} } public class ClassB {public string BField;}";
			var position = source.IndexOf("new ClassB().") + "new ClassB().".Length - 1;
			var symbols = new CompletionProvider().GetSymbols(source, position, new string[] { }, new string[] { }, LanguageNames.CSharp);
			//symbols = GetSymbols(source, position);
			symbols.Any(symbol => symbol.Name == "BField").ShouldBe(true);
			//symbols.Any(symbol => symbol.Name == "ClassA").ShouldBe(false);
		}
	}
}
