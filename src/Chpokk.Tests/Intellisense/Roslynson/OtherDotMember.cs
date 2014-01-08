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
	public class OtherDotMember : BaseCompletionTest {
		private const string STARTED_TYPING_NEW_CLASSB = "class ClassA { string field1; void method1(){new ClassB()./**/} } public class ClassB {public string BField;}";
		public OtherDotMember() : base(STARTED_TYPING_NEW_CLASSB) { }

		[Test]
		public void HasMemberNamesOfAnotherClassInIntellisenseData() {
			var symbols = GetSymbols();
			symbols.Any(symbol => symbol.Name == "BField").ShouldBe(true);
			//symbols.Any(symbol => symbol.Name == "ClassA").ShouldBe(false);
		}
	}
}
